using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatched to planet with unique gravity
//to work, must always have a GravityOrbit script attatched on Child with collider
public class GravityAttractor : MonoBehaviour
{
    //editor vars
    [SerializeField] float gravity = -10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2.5f;
    [SerializeField] bool fixedDirection;

    //system vars
    float rotSpeed = 20; 

    //class constructor
    public void SetVar(float pGravity, float pFallMultiplier, float pLowJumpMultiplier, bool pFixedDirection)
    {
        gravity = pGravity;
        fallMultiplier = pFallMultiplier;
        lowJumpMultiplier = pLowJumpMultiplier;
        fixedDirection = pFixedDirection;
        this.tag = "HardObject";
        this.gameObject.layer = 8;
    }

    Vector3 getTargetDir(Transform body) //gets target direction of the transform
    {
        if (fixedDirection)
            return transform.up; //tells to pull in the direction of the transform
        else
            return (body.position - transform.position).normalized; //tells to pull towards center of objectts
    }

    public void Attract(Transform body) //attracts the given transform towards the center and alters the rotation if it is not a fixed direction planet (such as a sphere)
    {
        Vector3 targetDir = getTargetDir(body); //gets target direction

        if (body.gameObject.tag != "Throwables") //doesn't rotate throwable objects
        {
            Quaternion targetRotation = Quaternion.FromToRotation(body.up, targetDir) * body.rotation; //calculates rotation based on FromToRot of the transform versus the target

            body.rotation = Quaternion.Lerp(body.rotation, targetRotation, rotSpeed * Time.deltaTime); // rotates the transform
        }

        if (body.gameObject.tag != "RocketProjectile") //doesnt pull down physical projectiles such as rockets 
            body.GetComponent<Rigidbody>().AddForce(targetDir * gravity); //adds the gravity value to pull towards the planet
    }

    public Vector3 FastFallAttract(Transform body) //fast fall
    {
        return getTargetDir(body) * gravity * (fallMultiplier - 1) * Time.deltaTime; //multiplies gravity on player to temporarily fall faster
    }

    public Vector3 LowJumpAttract(Transform body) //low jump
    {
        return getTargetDir(body) * gravity * (lowJumpMultiplier - 1) * Time.deltaTime; //adds additional force on player to stop jump
    }

}
