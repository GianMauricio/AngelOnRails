using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
//using UnityEngine.SceneManagement;

public enum PlayerState
{
    Moving,
    Shooting,
    Crouched,
    Done,
    Dead
}

public class PlayerManager : MonoBehaviour, ISwiped, ITwoFingerPan
{
    //Data
    public DataHolder data;
    public int Level;

    //Statics
    private static float moveSpeed = 0.05f;

    //Track all possible way-points, last way-point is automatically designated as exit
    public List<GameObject> Waypoints;
    public GameObject Player;
    public GameObject Eyes;

    //Track enemy status
    public GameObject WaveMaster;

    //Track current player state
    private PlayerState currState;

    //Track player position
    private Vector3 CurrPos;
    private Vector3 DestPos;
    private float DistFrac = 0;

    //Track player waypoint number
    private int nWaypointNum = 0;

    //Track touch info
    private Touch aFinger;
    private Touch bFinger;

    //Touch params
    public SwipeProperty _swipeProperty;
    public TwoFingerPanProperty _twoFingerPan;

    //Event params
    public event EventHandler<SwipeEventArgs> SwipeArgs;
    public event EventHandler<TwoFingerPanEventArgs> TwoFingerPanArgs;

    //Dynamic touch data
    private Vector2 start_pos;
    private Vector2 end_pos;
    private float gesture_time;

    //Track player timer stuff
    private float timeExposed = 0;
    private float MAXTIME = 1.0f; /*TODO:Make this not 1 second in final*/

    [Tooltip("Keep this really small or risk losing tap inputs")]
    private float tapTimer = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        //Confirm if every waypoint is tagged
        foreach (GameObject Target in Waypoints)
        {
            if (!Target.CompareTag("Waypoint") && !Target.CompareTag("TransitPoint"))
            {
                Debug.Log("No waypoint detected; how did you get here?");
            }
        }

        //Assume player starts in moving state
        currState = PlayerState.Moving;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timeExposed);
        //Debug.Log(nWaypointNum);
        //Check current player progress
        progressCheck();
        switch (currState)
        {
            //If the player is in transit
            case PlayerState.Moving:
                Player.GetComponent<WeaponTracker>().shiftState(true);
                //Move player until at next waypoint
                if (DistFrac < 1)
                {
                    DistFrac += Time.deltaTime * moveSpeed;
                    Player.GetComponent<Transform>().position = Vector3.Lerp(CurrPos, DestPos, DistFrac);
                }
                break;

            //If the player is in danger
            case PlayerState.Shooting:
                //Show shooting UI
                Player.GetComponent<WeaponTracker>().shiftState(false);

                //Always check if there are enemies left
                updateProgress();
                //Debug.Log(WaveMaster.GetComponent<EnemyCommander>().enemiesRemaining());
                if (WaveMaster.GetComponent<EnemyCommander>().enemiesRemaining() <= 0)
                {
                    currState = PlayerState.Moving;
                }

                //If tap/click is detected
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot(Input.mousePosition);
                }
            {
                /*
                

                //If rotation/R key is detected
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    Player.GetComponent<WeaponTracker>().tryReload();
                }

                //If swipe/C key is detected
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    //TODO:implement crouching
                }
                */


            }/*Legacy PC code*/

                //Process tap immediately
                if (Input.touchCount > 0)
                {
                    //If single gesture
                    if (Input.touchCount == 1)
                    {
                        aFinger = Input.GetTouch(0);
                        Shoot(aFinger.position);

                        if (aFinger.phase == TouchPhase.Began)
                        {
                            start_pos = aFinger.position;
                            gesture_time = 0;
                        }

                        if (aFinger.phase == TouchPhase.Ended)
                        {
                            end_pos = aFinger.position;

                            if (gesture_time <= _swipeProperty.MaxGestureTime && Vector2.Distance(start_pos, end_pos) >=
                                (_swipeProperty.MinSwipeDistance * Screen.dpi))
                            {
                                FireSwipeFunction();
                            }
                        }

                        else gesture_time += Time.deltaTime;
                    }

                    else if (Input.touchCount > 1)
                    {
                        aFinger = Input.GetTouch(0);
                        bFinger = Input.GetTouch(1);

                        //If fingers pan then crouch/uncrouch
                        if (aFinger.phase == TouchPhase.Moved && bFinger.phase == TouchPhase.Moved &&
                            Vector2.Distance(aFinger.position, bFinger.position) <= (_twoFingerPan.MaxDistance * Screen.dpi)) ;
                        {
                            FireTwoFingerPan();
                        }
                    }
                }


                //Logic for getting shot
                if (timeExposed >= MAXTIME)
                {
                    float incomingDamage = WaveMaster.GetComponent<EnemyCommander>().allowShot();
                    bool isDead = Player.GetComponent<WeaponTracker>().takeDamage(incomingDamage);

                    //Debug.Log(incomingDamage);
                    //Debug.Log(isDead);
                    timeExposed = 0f;

                    if (isDead)
                    {
                        data.invokeLevelEndUI(Level, false);
                    }

                    //else Debug.Log("Shot is non-fatal");
                }

                break;

            //If the player is crouched
            case PlayerState.Crouched: /*Will also revert to this state when not peeking*/
                //TODO: Show Info UI
                Debug.Log("Reverting timeExposed");
                timeExposed = 0f; /*Reset enemy shot counters*/
                break;

            case PlayerState.Done:
                timeExposed = 0f; /*Reset enemy shot counters*/
                break;

            default:
                Debug.Log("You broke the player");
                throw new ArgumentOutOfRangeException();
        }

        //TODO: make dead UI and anims
        timeExposed += Time.deltaTime;
    }

    //Called when the player needs to bang
    private void Shoot(Vector3 hitLoc)
    {
        //Determine which gun is being shot
        WeaponTracker gunStats = Player.GetComponent<WeaponTracker>();
        string currGun = gunStats.getType();
        float baseDamage = gunStats.getDamage();

        //Check for upgrades to gun type being used
        int gunRank = 1;

        //Make ray to go straight forward (No drop coz fuck that)
        Ray bullet = Eyes.GetComponent<Camera>().ScreenPointToRay(hitLoc);
        RaycastHit Impact;

        //Save whether or not the bullet was a killshot
        bool killedSomething = false;

        //If the player has ammo for that type, proceed to hit calculations
        if (gunStats.ammoLeft() > 0)
        {
            //Store hit data
            if (Physics.Raycast(bullet, out Impact))
            {
                //Debug.Log("Shot collided with: " + Impact.transform.tag);
                //Check if the thing hit was an actual enemy
                if (Impact.collider.CompareTag("BasicEnemy") || Impact.collider.CompareTag("ArmoredEnemy") || 
                    Impact.collider.CompareTag("ShieldedEnemy") || Impact.collider.CompareTag("ProtectedEnemy"))
                {
                    //Call enemy hit function giving data from native script Fuck that fucker up
                    killedSomething = Impact.collider.gameObject.GetComponent<EnemyLogic>().getHit(baseDamage, currGun, gunRank);
                }
            }

            //Update UI stuff
            gunStats.shotFired();

            //Ping audio boards
            gunStats.playAudio("Pew");
        }

        //Debug.Log(killedSomething);

        //if the shot killed, tell the UI about it
        if (killedSomething)
        {
            updateProgress();
        }
    }

    //Called when the player gets banged

    //Function dedicated to making the UI not shit and cry
    private void updateProgress()
    {
        //Set-up data
        WeaponTracker PlayerRef = Player.GetComponent<WeaponTracker>();
        EnemyCommander DirectorRef = WaveMaster.GetComponent<EnemyCommander>();
        int enemiesLeft = DirectorRef.enemiesRemaining(), 
            enemiesMax = DirectorRef.enemiesTotal();

        //Debug.Log(enemiesLeft + " / " + enemiesMax);
        PlayerRef.tryKill(enemiesLeft, enemiesMax);
    }

    //This controls the level progression
    public void progressCheck()
    {
        //If the player is not at the starting waypoint
        if (nWaypointNum == 0){
            //Start moving towards  starting waypoint
            CurrPos = Player.GetComponent<Transform>().position;
            DestPos = Waypoints[0].GetComponent<Transform>().position;
        }

        //If the player is done with the starting waypoint, but not at the exit waypoint yet
        else if (nWaypointNum < Waypoints.Count)
        {
            CurrPos = Player.GetComponent<Transform>().position;
            DestPos = Waypoints[nWaypointNum].GetComponent<Transform>().position;

        }
    }

    public void EndLevel()
    {
        Debug.Log("Arrived at exit");
        data.invokeLevelEndUI(Level, true);
    }

    //This shit happens when the player arrives at the waypoint
    public void Arrived(WaypointType waypoint)
    {
        //Set destination to next waypoint
        nWaypointNum++;

        //Reset Lerp counter
        DistFrac = 0;

        //Set new state to shooting, if the waypoint is an actual waypoint
        if (waypoint == WaypointType.Waypoint)
        {
            //Spawn new enemy wave
            Vector3 newOrientation = WaveMaster.GetComponent<EnemyCommander>().DeployNextWave();

            //Make player look at new wave location
            Camera.main.transform.LookAt(newOrientation);

            //Debug.Log("Changing player state");
            currState = PlayerState.Shooting;
        }

        //Increment progress level
        Player.GetComponent<WeaponTracker>().addProgress(nWaypointNum, Waypoints.Count);
    }

    //Touch functions
    public void OnSwipe(SwipeEventArgs swipeData)
    {
        WeaponTracker gunStats = Player.GetComponent<WeaponTracker>();
        //If down then reload
        if (swipeData.Direction == Directions.DOWN)
        {
            Player.GetComponent<WeaponTracker>().tryReload();

            //Ping audio boards
            gunStats.playAudio("Pew");
        }
    }

    public void OnTwoFingerPan(TwoFingerPanEventArgs panData)
    {
        //TODO: Dynamic crouch data (right/left if peeking; up/down if crouching)
        //If down then crouch
        if (panData.direction == Directions.DOWN)
        {
            currState = PlayerState.Crouched;
        }

        //If up then uncrouch
        if (panData.direction == Directions.UP && currState == PlayerState.Shooting)
        {
            currState = PlayerState.Shooting; /*Player cannot crouch while moving*/
        }
    }

    //Utility Functions
    private void FireSwipeFunction()
    {
        Debug.Log("SWIPE");

        Vector2 diff = end_pos - start_pos;

        GameObject hitObject = GetHit(start_pos);
        Directions dir;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > 0)
            {
                // Debug.Log("Right");
                dir = Directions.RIGHT;
            }
            else
            {
                //  Debug.Log("Left");
                dir = Directions.LEFT;
            }
        }
        else
        {
            if (diff.y > 0)
            {
                //Debug.Log("up");
                dir = Directions.UP;
            }

            else
            {
                //Debug.Log("down");
                dir = Directions.DOWN;
            }
        }

        SwipeEventArgs args = new SwipeEventArgs(start_pos, diff, dir, hitObject);

        if (SwipeArgs != null)
        {
            SwipeArgs(this, args);
        }

        this.OnSwipe(args);
    }

    private void FireTwoFingerPan()
    {
        Debug.Log("Panning");
        //Calculate for change vector
        Vector2 change = aFinger.position;

        //Logic here is that if the pan happens then aFinger is effectively == to bFinger
        Debug.Log("Change vector: " + change.x + " , " + change.y);

        TwoFingerPanEventArgs args = new TwoFingerPanEventArgs(aFinger, bFinger, change);

        if (TwoFingerPanArgs != null)
        {
            TwoFingerPanArgs(this, args);
        }
    }
    //External utility functions
    //TODO:Integrate these into the main project via other means
    private GameObject GetHit(Vector2 screenPos)
    {
        Ray r = Camera.main.ScreenPointToRay(start_pos);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        return hitObj;
    }
    private Vector2 GetPreviousPoint(Touch t)
    {
        return t.position - t.deltaPosition;
    }
    private Vector2 GetMidPoint(Vector2 p1, Vector2 p2)
    {
        Vector2 ret = new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        return ret;
    }
}
