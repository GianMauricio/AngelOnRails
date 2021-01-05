using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Track Player UI


    //Track player position
    private Vector3 CurrPos;
    private Vector3 DestPos;
    private float DistFrac = 0;

    //Track player waypoint number
    private int nWaypointNum = 0;

    //Are there still enemies in the current phase
    private bool bDangerClose = false;

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
                if (WaveMaster.GetComponent<EnemyCommander>().enemiesRemaining() <= 0)
                {
                    currState = PlayerState.Moving;
                }

                //TODO: Show Shooty bang bang UI
                //If tap/click is detected
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot(Input.mousePosition);
                }

                if (WaveMaster.GetComponent<EnemyCommander>().enemiesRemaining() <= 0)
                {
                    currState = PlayerState.Moving;
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
        string currGun = Player.GetComponent<WeaponTracker>().getType();
        float baseDamage = Player.GetComponent<WeaponTracker>().getDamage();

        //Check for upgrades to gun type being used
        int gunRank = 1;

        //Make ray to go straight forward (No drop coz fuck that)
        Ray bullet = Camera.GetComponent<Camera>().ScreenPointToRay(hitLoc);
        RaycastHit Impact;

        //Store hit data
        if (Physics.Raycast(bullet, out Impact))
        {
            Debug.Log("Shot collided with: " + Impact.transform.tag);
            //check if the thing hit was an actual enemy
            if (Impact.collider.CompareTag("BasicEnemy") || Impact.collider.CompareTag("ArmoredEnemy")
                || Impact.collider.CompareTag("ShieldedEnemy") || Impact.collider.CompareTag("ProtectedEnemy"))
            {
                //Debug damage set (IT FUCKING WORKS AHAHAHAHAHAH)
                //Destroy(Impact.collider.gameObject);

                //Call enemy hit function giving data from native script Fuck that fucker up
                Impact.collider.gameObject.GetComponent<EnemyLogic>().getHit(baseDamage, currGun, gunRank);
            }
        }
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
