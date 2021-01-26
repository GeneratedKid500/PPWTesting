using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiElementEnableDisable : MonoBehaviour
{
    public GameObject referencedWeapon;

    void LateUpdate()
    {
        if (!referencedWeapon.activeSelf) 
        {
            gameObject.SetActive(false); //deactivates itself upon referenced Weapon being disabled
        }

    }
}
