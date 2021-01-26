using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class ThrowablePlaySound : MonoBehaviour
{
    //editor vars
    public AudioClip hitFX;

    //system vars
    AudioSource audioSauce;
    float timer = 0;

    void Start()
    {
        audioSauce = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        timer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision obj)
    {
        //if is older than 0.1 seconds, plays sound effect when colliding 
        //with floor or enemy
        if (timer > 0.1f) 
        {
            switch (obj.gameObject.tag)
            {
                case "HardObject":
                case "Enemy":
                    audioSauce.PlayOneShot(hitFX);
                    break;
            }
        }
    }
}
