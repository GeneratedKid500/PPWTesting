using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowablesCollision : MonoBehaviour
{
    //editor vars
    public Collider thisCol;
    public float soundRadius = 10f;

    //system vars
    [HideInInspector] public ThrowableObjects parentObject;
    Rigidbody rb;
    float lifeTime;

    void Start()
    {
        lifeTime = 0;
        rb = GetComponent<Rigidbody>();
        thisCol = GetComponent<Collider>();
    }

    void Update()
    {
        rb.AddTorque(Vector3.forward); //adds some spin to the bottle
        lifeTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider obj)
    {
        //only allows collisions if the object is older than 0.1 seconds
        if (lifeTime > 0.1f)
        {
            switch (obj.tag)
            {
                case "Player":
                    //recollects if hit a player collider
                    parentObject.amountOf = parentObject.amountOf + 1; 
                    Destroy(this.gameObject);
                    break;

                default:
                    Debug.Log("Collided with " + obj.name); //returns any other collison to debug
                    break;
            }
        }
        else
        {
            Physics.IgnoreCollision(thisCol, obj, true);
        }
    }

}
