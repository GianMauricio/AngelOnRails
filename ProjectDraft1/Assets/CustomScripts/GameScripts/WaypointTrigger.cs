using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    public GameObject PlayerDirector;
    //If the object is entered by the player then tell the player it has reached the waypoint
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player has entered waypoint");
            PlayerDirector.GetComponent<PlayerManager>().Arrived();
        }
    }
}
