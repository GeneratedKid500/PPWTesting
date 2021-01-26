using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GravityControllerPCScript : MonoBehaviour
{
    //editor vars
    [SerializeField] GameObject screen;
    [SerializeField] Collider onCollider;
    [SerializeField] Collider[] offColliders;
    [SerializeField] Material altScreen;

    //system vars
    AudioSource audioSource;
    Renderer screenRend;
    TextMeshPro jargon;
    float timer = 0;
    bool GravityChanged = false;

    void Start()
    {
        jargon = GetComponentInChildren<TextMeshPro>(); //gets text for the PC screen
        audioSource = GetComponent<AudioSource>(); 
        screenRend = screen.GetComponent<Renderer>();
    }

    void Update()
    {
        if (GravityChanged)  //if bool gravity changed
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                //changes screen
                screenRend.material = altScreen;
                jargon.gameObject.SetActive(false);
                timer = 0;
                GravityChanged = false;
            }
        }
    }

    void ChangeGravity() //called in InteractRaycast.cs
    {
        if (!GravityChanged) //only interact with each PC the once
        {
            jargon.text = "c: / Users / Gemma / MyMusic / Britney / Gravity.exe"; //adds some text
            if (offColliders.Length > 0)
            {
                for (int i = 0; i < offColliders.Length; i++)
                {
                    //disables every collider in the array
                    offColliders[i].gameObject.SetActive(false); 
                }
            }

            //disables and re-enables collider to be activated 
            //so player will definietely be affected by it
            onCollider.gameObject.SetActive(false);
            onCollider.gameObject.SetActive(true);
            //adds more placeholder text on PC screen
            jargon.text = (jargon.text + "" +
                " Enabled Gravity Section " + onCollider.gameObject.name);

            audioSource.Play();

            GravityChanged = true;
        }
    }
}
