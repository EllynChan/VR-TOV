using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Changes the player's speed to a new speed

*/

public class ChangeSpeed : MonoBehaviour
{
    public float newSpeed;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().speed = newSpeed;
    }
}
