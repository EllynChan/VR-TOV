using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*

In the tutorial, after walking practice, reset player to its original position

*/

public class ResetPlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        SteamVR_Fade.View(Color.black,0.1f);
        player.GetComponent<Transform>().position = new Vector3(-21.4325f, -8.17f, 6.1f);
        player.GetComponent<Transform>().eulerAngles = new Vector3(0f, 0f, 0f);
        SteamVR_Fade.View(Color.clear, 5f);
    }
}
