using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatch to prefab of projectile
[RequireComponent (typeof(Rigidbody))]
public class ProjectileCollision : MonoBehaviour
{
    //editor vars
    public AudioClip explosion;
    public GameObject projectilePrefab;
    public GameObject projectileBody;
    public ParticleSystem collisionEffect;
    [Header ("Stats")]
    public int damage = 10;
    public int maxLifeTime = 5;

    //system vars
    AudioSource audioSource;
    Rigidbody rb;
    Collider thisCol;
    float lifeTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        thisCol = GetComponent<Collider>();

        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        //auto does this incase i forget

        lifeTime = 0;
    }

    void OnTriggerEnter(Collider collidedObject)
    {
        if (lifeTime > 0.1f) 
        {
            switch (collidedObject.tag)
            {
                case "Enemy":
                case "Enemy/Rushdown":
                case "Enemy/Rocket":
                    collidedObject.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver); //attack enemy
                    Explode(); //explode if it hits an enemy
                    break;

                case "Player":
                    Explode();
                    collidedObject.SendMessageUpwards("TakeDamage", SendMessageOptions.DontRequireReceiver); //hits player
                    break;

                case "HardObject":
                    Explode(); //explode if it hits a solid surface
                    break;
                //note to self - should just use layer detection to see if i hit surfaces

                default:
                    Debug.Log("Collided with " + collidedObject.name); //return collision
                    break;
            }
        }
        else
        {
            Physics.IgnoreCollision(thisCol, collidedObject, true);
        }

    }

    void LateUpdate()
    {
        lifeTime += Time.deltaTime;

        if (!collisionEffect.isPlaying)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
            rb.AddForce(transform.forward.normalized * 10f); //increase forward momentum by 10 once every frame 
            //simulates rocket speeding up
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0); //stop if collison effect is playing
        }
        Destroy(projectilePrefab, maxLifeTime); //auto destroy after a certain amount of time
    } 

    void Explode() //destroy when colliding and adds explosion effect
    {
        projectileBody.SetActive(false); //sets the main body inactive
        audioSource.PlayOneShot(explosion, 1f); //increases volume for explosion
        collisionEffect.Play(); //plays the explosion effect
        Destroy(projectilePrefab, 0.5f); //destroys gameobject after 0.5 seconds
    }
}
//right now physical projectiles aren't affected by gravity, which is a shame. Would love to work that out