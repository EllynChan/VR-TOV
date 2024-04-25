using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;
using System;
using System.Text;
using System.Linq; 
using System.IO;

/*
using System;
using TMPro;
*/

/**
 * Switch to the next scene after clicking >Next.
 * The scene sequence in each block(10 scenes) is random and will be shuffled after 
 * showing the entire block. There is a 30s intermission between each block.
 * 
 * The commented out parts of this is the implementation of the preious method, which is to
 * Switch to the next scene after some minTime. The timer starts right away.
 **/

public class SwitchSceneTransition : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    public string[] switchToScene;
    public GameObject intermissionScene;
    public GameObject ratingScene;
    public GameObject infoScene;
    public GameObject infoPanel;

/*
    public float minTime;
    public GameObject counterReferenceObject;
    public GameObject timerText;
    
    private float timer = 0.0f;
    private float currTime;
*/

    void Awake()
    {
        laserPointer.PointerClick += PointerClick;
    }


    void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button")
        {
            ShowRandomScene();
        }
    }

    void TeleportToScene(int i)
    {
        //entire scene fade to black within 0.1s.
        SteamVR_Fade.View(Color.black,0.1f);
        //load the scene to switch to; allows teleportation to change scene
        SceneManager.LoadScene(switchToScene[i]);
    }

    void ShowRandomScene()
    {
        if (StaticData.scenesIndices == null || StaticData.scenesIndices.Count == 0)
        {
            if (StaticData.index > 3)
            {
                Finish();
                return;
            } else if (StaticData.index != 0) {
                StartCoroutine(Intermission());
                return;
            } else {
                StartCoroutine(Instruction());
            } 
        } else {
            TeleportToScene(StaticData.scenesIndices.Pop());
        }
    }

    void FillScenesIndices()
    {
        System.Random random = new System.Random();
        StaticData.scenesIndices = new Stack<int>();
        for (int i = 0; i < switchToScene.Length; ++i)
            StaticData.scenesIndices.Push(i);
        StaticData.scenesIndices = new Stack<int>(StaticData.scenesIndices.OrderBy(x => random.Next()));
        StaticData.index++;
    }

    IEnumerator Intermission()
    {
        intermissionScene.SetActive(true);
        laserPointer.active = false; 
        ratingScene.GetComponent<Renderer>().enabled = false;
        StaticData.intermission = true;

        yield return new WaitForSeconds(30);

        StaticData.intermission = false;
        StartCoroutine(Info());
    }

    IEnumerator Instruction() {
        infoScene.SetActive(true);
        ratingScene.GetComponent<Renderer>().enabled = false;

        yield return new WaitUntil(finishInstruction);
        StartCoroutine(Info());
    }

    bool finishInstruction()
    {
        print(infoPanel.activeSelf);
        return infoPanel.activeSelf;
    }

    IEnumerator Info()
    {
        intermissionScene.SetActive(false);
        infoScene.SetActive(true);
        laserPointer.active = false;
        ratingScene.GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(5);
        
        FillScenesIndices();
        TeleportToScene(StaticData.scenesIndices.Pop());
    }

    //sets Encoding stage to finished and go to ObjectShow (Transfer of valence) stage
    void Finish()
    {
        StaticData.hasFinished = true;
        SteamVR_Fade.View(Color.black,0.1f);
        SceneManager.LoadScene("ObjectShow"); //object show room
    }
    
}
