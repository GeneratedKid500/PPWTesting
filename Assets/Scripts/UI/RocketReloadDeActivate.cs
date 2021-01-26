using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatched to UI element to indicate whether rocket launcher is reloaded or not
public class RocketReloadDeActivate : MonoBehaviour
{
    public GameObject RocketLauncher;

    void Update()
    {
        if (!RocketLauncher.activeSelf && RocketLauncher.GetComponent<ProjectileShoot>().isReloading)
            gameObject.SetActive(false); //deactivates itself upon rocket launcher being re-enabled
    }
}
