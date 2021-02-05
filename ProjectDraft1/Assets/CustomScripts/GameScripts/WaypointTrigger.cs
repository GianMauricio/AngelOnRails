using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointType{ Waypoint, Transit }

public class WaypointTrigger : MonoBehaviour
{
    public GameObject PlayerDirector;

    //If the object is entered by the player then tell the player it has reached the waypoint
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player has entered waypoint");
            if (gameObject.CompareTag("Waypoint"))
            {
                PlayerDirector.GetComponent<PlayerManager>().Arrived(WaypointType.Waypoint);
            }

            else if (gameObject.CompareTag("TransitPoint"))
            {
                PlayerDirector.GetComponent<PlayerManager>().Arrived(WaypointType.Transit);
            }

            else if (gameObject.CompareTag("ExitPoint"))
            {
                PlayerDirector.GetComponent<PlayerManager>().EndLevel();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDirector.GetComponent<PlayerManager>().Left();
        }
    }
}
