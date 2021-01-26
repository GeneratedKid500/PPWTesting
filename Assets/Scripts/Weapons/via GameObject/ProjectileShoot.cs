using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attatched to projectile firing systems
//// NOTE TO FUTURE ME - I intend to attatch this weapon to the enemy, so this script may be adapted a bit. Might be better making a new script
public class ProjectileShoot : MonoBehaviour
{
    //editor vars
    public GameObject projectilePrefab;
    public GameObject decorProjectile;
    public Scrollbar reloadVisual; //must have for reload visual
    //add the script UiElementEnableDisable.cs to the scrollbar 
    //for it to deactivate when you're scrolling through weapons
    [Header("Weapon Stats")]
    public int projectileCount = 1;
    public int reloadTime = 1;
    public float projectileSpeed = 500f;

    //system vars
    Vector3 spawnPos;
    Quaternion spawnRotation;
    int maxProjectiles;
    float reloadTimer;
    [HideInInspector] public bool isReloading;
    bool clickUp;

    void Start()
    {
        maxProjectiles = projectileCount; //maxmimum amount of projectiles = the starting amount 
    }

    void Update() //dealing with purely input here as movement is handled in ProjectileCollide.cs
    {
        ShootSubroutine(); 
        ReloadSubroutine(); 
    }

    void ShootSubroutine() //checks for shooting
    {
        if (Input.GetMouseButton(0) && projectileCount != 0 && clickUp)
        {
            clickUp = false;
            
            //sets the spawn Position and Rotation of the projectile
            spawnPos = decorProjectile.transform.position;
            spawnRotation = decorProjectile.transform.rotation;

            //summons an instance of the projectile prefab in the scene
            ///this may be adapted later to incorporate Object Pooling to be more memory efficient
            GameObject projectileInstance = Instantiate(projectilePrefab, spawnPos, spawnRotation); 
            projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed); //blasts it forward

            projectileCount--;
            decorProjectile.SetActive(false); //deactivates decor projectile on weapon
        }
        else if (!Input.GetMouseButton(0) && !clickUp)
            clickUp = true;

        //using GetMouseButton() rather than GetMouseButtonDown() 
        //offers more accurate click detection. 
        //bool clickUp makes it so that can only fire once as 
        //GetMouseButton() continually detects clicks
    }

    //this weaponis unique in that it takes time to reload after pressing button rather than instant 
    void ReloadSubroutine() //checks for reloading
    {
        if (Input.GetKeyDown(KeyCode.R) && projectileCount != maxProjectiles)
        {
            isReloading = true; //starts timer
            reloadVisual.gameObject.SetActive(true); //enables UI element scrollbar on player's Canvas
        }

        if (isReloading)
        {
            reloadTimer += Time.deltaTime; //starts timer
            reloadVisual.size = reloadTimer; //increases size of UI scrollbar on Canvas
            if (reloadTimer >= reloadTime)
            {
                projectileCount++; //adds one to the magazine B)
                decorProjectile.SetActive(true); //reinstates decor projectile on the model
                reloadVisual.gameObject.SetActive(false); //disables the UI scrollbar on Canvas
                reloadTimer = 0;
                isReloading = false; //disables this subroutine
            }

            if (gameObject.activeSelf)
                reloadVisual.gameObject.SetActive(true);
        }
        else
            reloadVisual.gameObject.SetActive(false);
    }
}
