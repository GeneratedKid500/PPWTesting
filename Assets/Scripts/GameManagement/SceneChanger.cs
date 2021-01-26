using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string changeToScene;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player") 
        {
            SceneManager.LoadScene(changeToScene);
        }
    }
}
