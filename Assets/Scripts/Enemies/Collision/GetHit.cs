using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    //editor vars
    public Material hurtMat;
    public float enemyHP = 50;

    //system vars
    Renderer rend;
    Material originMat;
    float timer;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originMat = rend.material; //gets enemy material
    }

    void Update() //damage indication
    {
        //if the current material is not the original material of the enemy
        if (rend.material != originMat) 
        {
            //turns it back after 0.1 seconds
            timer += Time.deltaTime;
            if (timer >= 0.1) 
            {
                rend.material = originMat;
                timer = 0;
            }
        }
    }

    void ApplyDamage(float damageDealt) //called in all damaging scrips
    {
        //takes away damage dealt
        enemyHP -= damageDealt;
        //changes material to the hurt material
        rend.material = hurtMat;

        //destroys object if HP is less than 0
        if (enemyHP <= 0) 
        {
                Destroy(gameObject);
        }
    }
}
