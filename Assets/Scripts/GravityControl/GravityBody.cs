using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached to player or anything to be affected by unique gravity
//object MUST have a collider to be affected by gravity
[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor planet;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (this.tag != "Throwables")
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        rb.useGravity = false;
        //auto does these incase I forget
        //doesnt freeze rotation of throwables 
    }

    void FixedUpdate()
    {
        if (planet)
            planet.Attract(transform);  
        //checks if the object or player is currently in a planets orbit
        //if it is then uses the attract function of GravityAttractor.cs - see it for more
    }
}
