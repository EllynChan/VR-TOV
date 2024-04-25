using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Valve.VR.Extras;


/**
 * Handles movement using the joystick and pausing the experiment
 **/
public class PlayerController : MonoBehaviour
{

    public SteamVR_Action_Vector2 input;
    public SteamVR_Action_Boolean pause;
    public GameObject pausePanel;
    public SteamVR_LaserPointer laserPointer;
    public float speed;
    
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        speed = 1; 

        if(SceneManager.GetActiveScene().name == "Tutorial" || 
            SceneManager.GetActiveScene().name == "Transition" ||
            SceneManager.GetActiveScene().name == "ObjectShow") 
        {
            speed = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(input.axis.magnitude > 0.1f && !StaticData.gamePaused)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0,9.81f,0) * Time.deltaTime);
        }

        if (!StaticData.intermission)
        {
            if (pause.lastStateDown && !StaticData.gamePaused)
            {
                StaticData.gamePaused = true;
                Time.timeScale = 0.0f;
                pausePanel.SetActive(true);
                laserPointer.active = false;
            } 

            else if (pause.lastStateDown && StaticData.gamePaused)
            {
                StaticData.gamePaused = false;
                Time.timeScale = 1.0f;
                pausePanel.SetActive(false);
                laserPointer.active = true;
            }
        } 
    }
}

