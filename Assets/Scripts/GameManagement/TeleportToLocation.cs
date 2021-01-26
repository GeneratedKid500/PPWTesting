using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToLocation : MonoBehaviour
{
    public Vector3 location;

    //teleports player to specified location when a collider is entered
    //used to stop player from falling out of map
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Transform>().position = location; 
        }   
    }
}
