using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatched to the orbit collider of an object with a GravityAttractor.cs
public class GravityOrbit : MonoBehaviour
{
    public float setGravity = -10f;
    public float setFallMultiplier = 2.5f;
    public float setLowJumpMultiplier = 2f;
    public bool setFixedDirection;

    void Awake()
    {
        if (!GetComponentInParent<GravityAttractor>())
        {
            transform.parent.gameObject.AddComponent<GravityAttractor>();
            transform.parent.gameObject.GetComponent<GravityAttractor>().SetVar(setGravity, setFallMultiplier, setLowJumpMultiplier, setFixedDirection); //uses constructor
        }
        this.gameObject.layer = 2;
        //adds a gravity attractor to the parent of this if it doesnt already have one
        /// see GravityAttractor.cs for more
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.GetComponent<GravityBody>())
        {
            obj.GetComponent<GravityBody>().planet = this.GetComponentInParent<GravityAttractor>();
        }
        //if an object with a gravity body enters the collider, sets the GravityAttractor of the parent to that objects gravity body
    }
}
