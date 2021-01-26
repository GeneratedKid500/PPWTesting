using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractRaycast : MonoBehaviour
{
    //edito vars
    public Image panel;
    public AudioClip victoryMusic;

    //system vars
    float timer;
    bool isInteracting = false;
    bool hasWon = false;

    // Update is called once per frame
    void Update()
    {
        //Detects if right mouse button is clicked down
        if (Input.GetMouseButtonDown(1)) 
        {
            isInteracting = true;  //sets bool to true
            Debug.Log("Got input");
        }

        //in relation to right clicking on the treasure
        if (hasWon)
        {
            //plays victory theme in Music Manager
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayClip(victoryMusic); 
            panel.CrossFadeAlpha(2f, 3f, true); //fades in blue panel over 3 seconds, unaffected by scaled time
            timer += Time.unscaledDeltaTime; //like delta time but not affected by scaled time
            if (timer > 3f)
            {
                SceneManager.LoadScene("Victory"); //loads victory scene
            }
        }
    }

    void FixedUpdate() //shooting raycast
    {
        //if right click down
        if (isInteracting)
        {
            //sends out raycast for 2m
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f))
            {
                //checks tag
                switch (hit.transform.gameObject.tag) 
                {
                    case "GravitySwitcher":
                        hit.collider.SendMessageUpwards("ChangeGravity"); //see GravityControllerPCScript.cs
                        break;

                    case "Treasure":
                        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().StopMusic();  //stops bgm
                        panel.gameObject.SetActive(true); //activates panel
                        panel.GetComponent<CanvasRenderer>().SetAlpha(0f); //sets panel as invisible
                        panel.color = Color.blue; //changes colour to blue
                        Time.timeScale = 0; //stops time
                        hasWon = true; //change bool
                        break;

                    default:
                        //return any other raycast collision
                        Debug.Log("Collided with " + hit.transform.gameObject.name);
                        break;
                } 
            }

            isInteracting = false; //resets isInteracting bool
        }    
    }
}
