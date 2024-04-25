using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

/**
 * Closes selected (current) panel and opens the next panel upon clicking
 * a button (button must be named Button).
 **/

public class SwitchPanel : MonoBehaviour
{
    public GameObject panelToOpen;
    public GameObject panelToClose;
    public SteamVR_LaserPointer laserPointer;

    void Awake()
    {
        laserPointer.PointerClick += PointerClick;
    }

    void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button")
        {
            //Debug.Log("Button was clicked");
            Switch();
        }
    }

    void Switch()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }

        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
        }
    }
}
