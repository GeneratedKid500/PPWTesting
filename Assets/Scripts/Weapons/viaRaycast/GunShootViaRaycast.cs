using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Attatch to weapon parent
public class GunShootViaRaycast : MonoBehaviour
{
    ////editor Vars
    public GameObject gunShotFX;
    public TextMeshPro bulletCounter;

    [Header("Gun Variables")]
    public int bulletCount = 8;
    public int damage = 10;
    public int travelDistance = 20;
    public float fireRate = 0.3f;
    public bool Automatic = false;

    ////system Vars
    AudioSource audioSource;
    int maxBulletCount;
    float fireDelay;
    bool clickUp;
    //fx
    float fxTimer;
    bool fxOn;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxBulletCount = bulletCount; //sets maximum amount of bullets
        bulletCounter.text = bulletCount.ToString(); //updates TMPro
    }

    void Update()
    {
        ///RELOAD FUNCTION
        if (Input.GetKeyDown(KeyCode.R) && bulletCount != maxBulletCount)
        {
            if (bulletCount != 0)
                bulletCount = maxBulletCount + 1; //bullet already in chamber when magazine is refreshed
            else
                bulletCount = maxBulletCount; //chamber empty so no extra bullet
            bulletCounter.text = bulletCount.ToString(); //update TMPro
        }
        ShotFX(); //called to keep timer active
    }

    void FixedUpdate() //bullets use raycasts
    {
        if ((!Automatic && Input.GetMouseButton(0) && Time.fixedTime > fireDelay && bulletCount > 0 && clickUp) || //semi auto 
            (Automatic && Input.GetMouseButton(0) && Time.fixedTime > fireDelay && bulletCount > 0)) //full auto
        {
            fireDelay = Time.time + fireRate;
            FireShot();
            audioSource.Play();
            clickUp = false;
        }
        else if (!Input.GetMouseButton(0) && !clickUp)  
        {
            clickUp = true;
            //bool clickUp enforces manual shooting in semi auto guns
            //whilst still offering better click detection than GetMouseButtonDown()
        }
    }

    void FireShot()
    {
        Vector3 directionOfTravel = transform.TransformDirection(Vector3.forward); //fires raycast in current direction
        fxOn = true; //fx

        if (Physics.Raycast(transform.position, directionOfTravel, out RaycastHit hit, travelDistance))
        {
            Debug.Log("Hit " + hit.collider.name); //prints what hit into debug log
            Debug.DrawLine(transform.position, hit.point, Color.magenta); //draws line of fire

            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver); //sends damage message
        }
        bulletCount--;
        bulletCounter.text = bulletCount.ToString(); //updates bullet count on TextMeshPro every time shot is fired
    }

    //fx
    void ShotFX()
    {
        if (fxOn)
        {
            fxTimer += Time.deltaTime;
            gunShotFX.SetActive(true); //activates gameObject

            if (fxTimer > 0.05)
            {
                fxTimer = 0; 
                gunShotFX.SetActive(false);
                fxOn = false;
            }
        }
    }
}
