using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatch to any particles you want to deal damage
public class ParticleCollision : MonoBehaviour
{
    public float damage = 0.2f;

    private void OnParticleCollision(GameObject collison) //returns value for once every bunch of particles rather than every single one - is less intensive
    {
        if (collison.tag == "Enemy" || collison.tag == "Enemy/Rushdown" || collison.tag == "Enemy/Rocket")
        {
            collison.GetComponent<CapsuleCollider>().SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver); //see GetHit.cs
        }
        else if (collison.tag != "Weapon/Fire" && collison.gameObject.layer != 2) 
        {
            Debug.Log("Has Collided with " + collison.name); //returns name of whatever it collided with as long as its not got the same tag as itself OR ignoreRaycast layer
        }
    }
}
