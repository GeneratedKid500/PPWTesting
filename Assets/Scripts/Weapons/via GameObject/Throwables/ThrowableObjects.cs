using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableObjects : MonoBehaviour
{
    //editor vars
    public GameObject throwPrefab;
    public GameObject decorThrowable;
    public Text amountLeftDisplay;
    public int amountOf = 3;
    public int reloadTime = 1;
    public int baseThrowForce = 500;
    public float increaseRate = 1f;

    //system vars
    Rigidbody playerRB;
    Renderer[] throwRend;
    Vector3 spawnPos;
    Quaternion spawnRotation;
    int maxForce;
    float actualThrowForce;
    float reloadTimer;
    bool isReloading;
    bool clickUp = true;

    void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        throwRend = decorThrowable.GetComponentsInChildren<Renderer>();

        amountLeftDisplay.text = "Left: " + amountOf;

        maxForce = 1000;
        actualThrowForce = baseThrowForce;
    }

    void Update()
    {
        //if player presses down left mouse and has more than 1 throwable
        //will increase the amount of force the bottle will be thrown with
        //until it is larger or equal to the maximum force (1000)
        if (Input.GetMouseButton(0) && amountOf > 0)
        {
            if (actualThrowForce < maxForce)
                actualThrowForce += increaseRate;
            clickUp = false;
        }

        UiHandler();
    }

    void FixedUpdate()
    {
        //once the player releases the left mouse button
        //summons new object, removes one from inventory
        //and resets the throw force to minimum
        if (!Input.GetMouseButton(0) && clickUp == false)
        {
            clickUp = true;
            ThrowSubroutine();

            amountOf--;
            actualThrowForce = baseThrowForce;
        }
    }

    void ThrowSubroutine() 
    {
        //sets the position and rotation of the new object to be spawned in
        spawnPos = decorThrowable.transform.position;
        spawnRotation = decorThrowable.GetComponentInChildren<Transform>().rotation;

        //summons an instance of the projectile prefab in the scene
        ///this may be adapted later to incorporate Object Pooling to be more memory efficient
        GameObject throwableInstance = Instantiate(throwPrefab, spawnPos, spawnRotation);
        throwableInstance.GetComponent<Rigidbody>().AddForce(transform.forward * (actualThrowForce)); //blasts it forward
        throwableInstance.GetComponent<ThrowablesCollision>().parentObject = this;
    }

    void UiHandler() 
    {
        //activates UI element showing how many items are left
        //when this item reactivates
        if (gameObject.activeSelf)
            amountLeftDisplay.gameObject.SetActive(true);

        //checks amount left and updates UI element text
        if (amountOf == 0)
        {
            // red if none left
            amountLeftDisplay.color = Color.red;
            amountLeftDisplay.text = "None Left";
        }
        else 
        {
            //white if otherwise
            amountLeftDisplay.text = "Left: " + amountOf;
            amountLeftDisplay.color = Color.white;
        }

        //deactivates the renderers in the children of the primary objects
        // to make it seem like no objects are left to throw
        // until one more is collided with
        if (amountOf == 0 && throwRend[2].gameObject.activeSelf)
        {
            for (int i = 0; i < throwRend.Length; i++)
                throwRend[i].gameObject.SetActive(false);
        }
        else if (amountOf > 0 && !throwRend[2].gameObject.activeSelf) 
        {
            for (int i = 0; i < throwRend.Length; i++)
                throwRend[i].gameObject.SetActive(true);
        }
    }

}
