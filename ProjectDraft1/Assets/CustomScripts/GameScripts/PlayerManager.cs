using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerManager : MonoBehaviour
{
    //Statics
    private static float moveSpeed = 0.05f;

    //Track all possible way-points, last way-point is automatically designated as exit
    public List<GameObject> Waypoints;
    public GameObject Player;
    public GameObject Camera;

    //Track enemy status
    public GameObject WaveMaster;

    //Track current player state
    private enum PlayerState
    {
        Moving,
        Shooting,
        Crouched,
    }
    private PlayerState currState;

    //Track player position
    private Vector3 CurrPos;
    private Vector3 DestPos;
    private float DistFrac = 0;

    //Track player waypoint number
    private int nWaypointNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Confirm if every waypoint is tagged
        foreach (GameObject Target in Waypoints)
        {
            if (!Target.CompareTag("Waypoint") && !Target.CompareTag("TransitPoint"))
            {
                Debug.Log("Designated waypoint has no type, movement will lock");
            }
        }

        //Assume player starts in moving state
        currState = PlayerState.Moving;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(nWaypointNum);
        //Check current player progress
        progressCheck();
        switch (currState)
        {
            case PlayerState.Moving:
                //TODO: Hide Shooty bang bang UI
                //Move player until at next waypoint
                if (DistFrac < 1)
                {
                    DistFrac += Time.deltaTime * moveSpeed;
                    Player.GetComponent<Transform>().position = Vector3.Lerp(CurrPos, DestPos, DistFrac);
                }

                break;

            case PlayerState.Shooting:
                //Always check if there are enemies left
                updateProgress();
                if (WaveMaster.GetComponent<EnemyCommander>().enemiesRemaining() <= 0)
                {
                    currState = PlayerState.Moving;
                }

                //If tap/click is detected
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot(Input.mousePosition);
                }

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

                break;

            case PlayerState.Crouched: /*Will also revert to this state when not peeking*/
                //TODO: Show Info UI
                break;
        }
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
        Ray bullet = Camera.GetComponent<Camera>().ScreenPointToRay(hitLoc);
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
        }

        //Debug.Log(killedSomething);

        //if the shot killed, tell the UI about it
        if (killedSomething)
        {
            updateProgress();
        }
    }

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
    void progressCheck()
    {
        //If the player is not at the starting waypoint
        if (nWaypointNum == 0){
            //Start moving towards  starting waypoint
            CurrPos = Player.GetComponent<Transform>().position;
            DestPos = Waypoints[0].GetComponent<Transform>().position;
        }

        //If the player is done with the starting waypoint, but not at the exit waypoint yet
        else if (nWaypointNum < Waypoints.Count - 1)
        {
            CurrPos = Player.GetComponent<Transform>().position;
            DestPos = Waypoints[nWaypointNum].GetComponent<Transform>().position;

        }

        //If the next waypoint is the exit
        else if (nWaypointNum == Waypoints.Count)
        {
            CurrPos = Player.GetComponent<Transform>().position;
            DestPos = Waypoints[nWaypointNum].GetComponent<Transform>().position;


            //TODO: Call UI script and end level
        }
    }

    //This shit happens when the player arrives at the waypoint
    public void Arrived()
    {
        //Spawn new enemy wave
        WaveMaster.GetComponent<EnemyCommander>().DeployNextWave();

        //Set destination to next waypoint
        nWaypointNum++;

        //Reset Lerp counter
        DistFrac = 0;

        //Set new state to shooting, if the waypoint is an actual waypoint
        currState = PlayerState.Shooting;
    }
}
