using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Allows object to move through a list of MoveSpots in order with no pause 
 * in between. Used for the movement of the spider.
 **/
public class Patrol : MonoBehaviour
{

    public float speed;

    public Transform[] moveSpots;
    private int moveSpotIndex;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        //make object go towards first moveSpot
        moveSpotIndex = 0;
        transform.LookAt(moveSpots[moveSpotIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        //allows bigger range of area for the movement to be counted as complete
        distance = Vector3.Distance(transform.position, moveSpots[moveSpotIndex].position);
        if(distance < 1f)
        {
            IncreaseIndex();
        }

        Patroling();
    }

    void Patroling()
    {
        //the actual movement
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    void IncreaseIndex()
    {
        //increase index, return to beginning if at end. Make object look in position of new index
        moveSpotIndex++;
        if(moveSpotIndex >= moveSpots.Length)
        {
            moveSpotIndex = 0;
        }

        transform.LookAt(moveSpots[moveSpotIndex].position);
    }

}
