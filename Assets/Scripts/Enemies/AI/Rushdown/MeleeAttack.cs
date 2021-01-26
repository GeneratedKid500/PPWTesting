using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //editor vars
    public GameObject meleeWeapon;
    public int rotationSpeed = 10;

    //system vars
    AudioSource audioSource;
    Transform originalPos;
    int maxHitPerSwing = 1;
    int hitsOnSwing = 0;
    int counter = 0;
    float countLimit = 70;
    bool striking = false, retracting = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 

        originalPos = meleeWeapon.transform;

        countLimit /= rotationSpeed;

        meleeWeapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MeleeStrike();
    }


    private void OnTriggerEnter(Collider obj)
    {
        if (hitsOnSwing < maxHitPerSwing)
        {
            hitsOnSwing++;
            obj.SendMessageUpwards("TakeDamage", SendMessageOptions.DontRequireReceiver);
            //Debug.Log("Hit Player!");
        }
    }

    void MeleeStrike()
    {
        if (striking) //moving melee weapon forward
        {
            counter++; //starts adding to counter
            meleeWeapon.transform.Rotate(Vector3.up, rotationSpeed); //starts moving weapon forward
            if (counter >= countLimit)
            {
                striking = false;
                audioSource.Play();
                retracting = true; //stops moving weapon forwrard once timer reaches above value
            }
        }
        else if (retracting) //moving melee weapon backwards
        {
            counter--; //starts removing to the counter
            meleeWeapon.transform.Rotate(Vector3.up, -rotationSpeed); //starts moving weapon backward 
            if (counter <= 0) //if counter is less than 0
            {
                counter = 0; //resets counter at 0
                retracting = false; //disables melee system
                meleeWeapon.transform.position = originalPos.position;
            }
        }
        else 
        {
            if (!striking && !retracting && Time.timeScale > 0) 
            {
                striking = true;
                hitsOnSwing = 0;
            }
        }
    }
}
