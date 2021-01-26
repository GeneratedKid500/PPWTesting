using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatch to a variation of the melee weapon game object that is only active when melee attacks are being dealt
///Note to future me - I could actually deactivate and reactivate this script when the player attacks. 
public class MeleeStrike : MonoBehaviour
{
    //editor vars
    public int maxHitsPerSwing = 1;
    public int damage = 15;

    //system vars
    [HideInInspector] public int hitCounter = 0;

    void OnTriggerEnter(Collider collidedWith)
    {
        switch (collidedWith.tag) 
        {
            case "Enemy":
            case "Enemy/Rushdown":
            case "Enemy/Rocket":
                if (hitCounter < maxHitsPerSwing)
                {
                    collidedWith.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver); //see GetHit.cs 
                    hitCounter++; //increases hitcounter - see explanation below
                }
                break;

            default:
                if (collidedWith.gameObject.layer != 2) //if the layer of the gameobject that was collided with is not "IgnoreRaycasts"
                    Debug.Log("Collided with " + collidedWith.name); //then return what the name of that it was
                break;
        }
        //maxHitsPerSwing acts as a buffer, as the animation could cause a melee weapon's collider to exit and enter another collider multiple times
        //this gives the developer control over the maxiumum amount of hits a melee weapon can deal
    }
}
