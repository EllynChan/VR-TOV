using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using System;
using TMPro;

/**
 * Switch to the designated scene after some minTime. 
 * The timer only starts once the highlights (counterRefereceObject)
 * becomes active after Player reaches the circle
 * 
 * Attach this script to Player.
 **/
public class SwitchSceneAutomatic : MonoBehaviour
{
    
    public string switchToScene;
    public GameObject counterReferenceObject;
    // public GameObject timerText; // this is if you want to show countdown on screen

    private float timer = 0.0f;
    private float currTime;
    private float awakeTime;
    private float minTime = 10; 

    // Start is called before the first frame update
    void Start()
    {
        currTime = minTime;
        awakeTime = Time.time;
        // StaticData.index = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (counterReferenceObject.activeSelf)
        {
            StaticData.timeToCircle = Time.time - awakeTime;
            timer += Time.deltaTime;

            if (currTime > 1)
            {
                currTime -= Time.deltaTime;
                
                //this line is to show the countdown on screen
                //timerText.GetComponent<TMP_Text>().text = Mathf.FloorToInt(currTime % 60).ToString();
            }
        }

        if (timer > minTime)
            SwitchToScene();
    }

    void SwitchToScene()
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
