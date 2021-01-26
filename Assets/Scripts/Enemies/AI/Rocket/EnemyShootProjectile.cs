using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectile : MonoBehaviour
{
    //edior vars
    public GameObject projectilePrefab;
    public GameObject decorProjectile;
    [Header("Weapon Stats")]
    public int projectileCount = 1;
    public float reloadTime = 1.5f;
    public float projectileSpeed = 500f;

    //system vars
    Vector3 spawnPos;
    Quaternion spawnRotation;
    float reloadTimer;
    bool isReloading;

    void Update()
    {
        EnemyShootSRT();
        EnemyReloadSRT();
    }

    void EnemyShootSRT() //enemy shoots
    {
        if (projectileCount != 0) 
        {
            //similar to player rocket shoot
            //gets position and rotation 
            spawnPos = decorProjectile.transform.position;
            spawnRotation = decorProjectile.transform.rotation;

            //summons object at that position
            GameObject projectileInstance = Instantiate(projectilePrefab, spawnPos, spawnRotation);
            projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);


            projectileCount--;
            decorProjectile.SetActive(false); //deactivates decor projectile on weapon
        }
    }

    void EnemyReloadSRT() //enemy reloads
    {
        //if projectiles is less than the max (1), begisn reloading
        if (projectileCount == 0) 
        {
            isReloading = true;
        }
        //if not, backs out
        else
            return;

        //reloads after specified amount of time
        //allows enemy to shoot again
        if (isReloading) 
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime) 
            {
                projectileCount++;
                decorProjectile.SetActive(true);
                reloadTimer = 0;
                isReloading = false;
            }
        }
    }

}
