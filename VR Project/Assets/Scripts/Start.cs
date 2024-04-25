using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

/**
 * On button press, move to a scene. Used to move from tutorial room to Encoding.
 **/
public class Start : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    public string switchToScene;

    void Awake()
    {
        laserPointer.PointerClick += PointerClick;
    }

    void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button")
        {
            //Debug.Log("Button was clicked");
            StartGame();
        }
    }

    void StartGame()
    {
        if ( !string.IsNullOrEmpty( switchToScene ) )
        {   
            //entire scene fade to black within 0.1s.
            SteamVR_Fade.View(Color.black,0.1f);
            //load the scene to switch to; allows teleportation to change scene
            SceneManager.LoadScene(switchToScene); 
        }
    }
}
