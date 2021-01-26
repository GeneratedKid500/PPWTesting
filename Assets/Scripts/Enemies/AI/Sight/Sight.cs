using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should be attatched to enemies with a FOV
[RequireComponent (typeof (AudioSource))]
[RequireComponent (typeof (Targeting))]
public class Sight : MonoBehaviour
{
    //editor vars
    [Header("FOV")]
    public float angle = 120f;
    public float radius = 10f;

    [Header("Alert System")]
    public GameObject alertSign;
    public GameObject confusedSign;
    public float confusedTimer = 1;
    public AudioClip alertSound;

    public Vector3 fromVector
    {
        get
        {
            float leftAngle = -angle / 2;
            leftAngle += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(leftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(leftAngle * Mathf.Deg2Rad));
        }
    }
    //gets the left angke of the radius of which the enemy can see the player and then uses that to form the visual seen in editor FOV

    //system vars
    Targeting targeting;
    GravityBody playerGrav;
    GravityBody myGrav;
    AudioSource myAudioSource;
    [HideInInspector] public GameObject player;
    float distance;
    float timer = 0;

    void Start()
    {
        myGrav = GetComponent<GravityBody>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerGrav = player.GetComponent<GravityBody>();
        targeting = GetComponent<Targeting>();
        targeting.enabled = false; //ensures enemy is not instantly targeting player
        myAudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() //deals with raycasts
    {
        Vector3 directionVector = (transform.position - player.transform.position).normalized; //calculated rotational direction to player
        distance = Vector3.Distance(transform.position, player.transform.position); //calculates distance from player

        float dotProduct = Vector3.Dot(directionVector, transform.forward); //checks the dot of the direction to player and the forward direction

        if (dotProduct < -0.5f && distance < radius && playerGrav.planet == myGrav.planet)
        {
            Vector3 playerPos = player.transform.position - transform.position; //gets players pos

            if (Physics.Raycast(transform.position, playerPos, out RaycastHit hit, radius) && hit.transform.tag == "Player") //checks if can hit player with Raycast
            {
                timer += Time.fixedDeltaTime;
                targeting.enabled = true; //turns to face player
                if (timer < confusedTimer)
                {
                    if (!confusedSign.activeInHierarchy) 
                    {
                        confusedSign.SetActive(true); //enables confused icon
                    }
                    //for the first few seconds (or specificed time slot) of the enemy seeing the player
                    //they will be confused - thats this
                }
                else
                {
                    if (confusedSign.activeInHierarchy) 
                        confusedSign.SetActive(false); //deactivates confused icon
                    else if (!alertSign.activeInHierarchy) 
                    {
                        alertSign.SetActive(true); //may also activate alert icon
                        myAudioSource.PlayOneShot(alertSound);
                    }

                    //after they are confused, if they can still see the player they will enter 
                    //their alert state
                }
            }
            else
            {
                TurnOffAlert();
            }
        }
        else
        {
            TurnOffAlert();
        }

        //this is used twice as it can be called from either state but potentially only one of them 
        //~~this was the easiest fix lol~~

        void TurnOffAlert()
        {
            if (confusedSign.activeInHierarchy)
                confusedSign.SetActive(false); //disables enemies' confused indicator if it is on
            alertSign.SetActive(false); //disables enemies' alert indicator
            targeting.enabled = false; //disables targeting script
            timer = 0;
        }
    }
}
