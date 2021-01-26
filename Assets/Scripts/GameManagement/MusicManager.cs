using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    void Awake()
    {
        //prevents the object from being destroyed between scenes
        //allows music to play across scenes such as game over or win screens
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    //plays aduio passed into function
    public void PlayClip(AudioClip clip) 
    {
        if (audioSource.isPlaying) return;

        audioSource.PlayOneShot(clip);
    }

    //stops the current track
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
