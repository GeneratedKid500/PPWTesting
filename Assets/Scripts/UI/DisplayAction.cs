using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAction : MonoBehaviour
{
    public Text displayAction;

    //activates UI element when in collider
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            displayAction.gameObject.SetActive(true);
        }

    }

    //deactivates UI element when leaving collider
    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            displayAction.gameObject.SetActive(false);
        }

    }
}
