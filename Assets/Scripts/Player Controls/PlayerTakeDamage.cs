using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerTakeDamage : MonoBehaviour
{
    //editor vars
    public AudioClip GameOverSong;
    public Image panel;
    public int recoveryTime = 5;

    //system vars
    FirstPersonController playerMove;
    WeaponSwitch playerSwitch;
    CanvasRenderer renderCanv;
    float timer = 0;
    float deathTimer;
    bool hit;
    [HideInInspector] public bool dead;

    void Start()
    {
        renderCanv = panel.GetComponent<CanvasRenderer>();
    }

    void Update()
    {
        //if the player is hit but now dead
        if (hit && !dead)
        {
            //increases normal timer
            timer += Time.deltaTime;
            //fades out panel on UI for a specified amount of time
            panel.CrossFadeAlpha(0f, recoveryTime, false);
            //deactivates panel and this if once time has been allotted
            if (timer >= recoveryTime)
            {
                hit = false;
                panel.gameObject.SetActive(false);
                timer = 0;
            }
        }
        // if the player is dead 
        else if (dead)
        {
            //plays the game over theme
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayClip(GameOverSong);
            panel.CrossFadeAlpha(2f, 3f, true); //fades in the UI panel for 3 seconds
            deathTimer += Time.unscaledDeltaTime; //increases timer using unscaledDeltaTime so not affected by scale
            if (deathTimer > 3f)
            {
                //transitions to new scene after allotted amount of time
                SceneManager.LoadScene("Game-Over");
            }
        }
    }

    void TakeDamage() //referenced in ProjectileCollision and EnemyMelee
    {
        //if the player takes damage when already hit, dies
        if (hit)
        {
            Die();
        }
        // if player has not been hit yet 
        else
        {
            //activates red panel show has been hit
            panel.gameObject.SetActive(true);
            renderCanv.SetAlpha(1f);
            hit = true;
        }
    }

    void Die()
    {
        //reduces time scale, stops current music
        Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().StopMusic();
        //sets panel to black and activates bool dead
        panel.gameObject.SetActive(true);
        panel.color = Color.black;
        dead = true;
        hit = false;
    }
}
