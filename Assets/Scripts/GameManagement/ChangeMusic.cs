using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangeMusic : MonoBehaviour
{
    public AudioClip clip;

    MusicManager musicManager;
    void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
    }

    //uses music manager to change the currently played track
    //when the player enters a trigger
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            musicManager.StopMusic();
            musicManager.PlayClip(clip);
        }
    }
}
