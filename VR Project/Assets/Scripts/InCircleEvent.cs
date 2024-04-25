using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Handles detection of reaching the circle and highlights the object pair.
 * Drag in highlights that need to be activated, and the audio that should
 * play once player reach the circle.
 * 
 * The way this script detects whether the player reached the circle is
 * through the player's distance to the center of the circle. If the 
 * distance is smaller than the radius of the circle then it means the
 * player has reached the circle. 
 * 
 * Player must be tagged Player! 
 * 
 * The radius is different in each scene since the circles are of 
 * different size. Make sure the circle game object has a mesh collider
 * attached for the radius to calculate properly!
 * 
 * This script records the scene name and the object names for the Encoding csv
 **/

public class InCircleEvent : MonoBehaviour
{
    private Transform player;
    private Vector3 initPosition;
    private Transform target;
    private bool soundPlayed = false;
    private float radius;
    private GameObject objOne;
    private GameObject objTwo;
    private List<Vector2> transformList = new List<Vector2>();

    public AudioClip audio;
    public GameObject[] highlights;
    public GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initPosition = player.position;
        objOne = GameObject.FindGameObjectWithTag("Object1");
        objTwo = GameObject.FindGameObjectWithTag("Object2");
        target = this.transform;
        radius = this.GetComponent<Collider>().bounds.size.x / 2;
        InvokeRepeating("RecordTransform", 0f, 1f);

        StaticData.scene = SceneManager.GetActiveScene().name;
        StaticData.objectOne = objOne.name;
        StaticData.objectTwo = objTwo.name;
    }

    void UpdateTransparency(float d)
    {
        var solidDistance = radius + 1.5f;

        if (d < solidDistance) d = solidDistance;

        var colorOrig = sphere.GetComponent<MeshRenderer>().material.color;
        colorOrig.a = 1 - (Mathf.Pow(2.5f, solidDistance) / Mathf.Pow(2.5f, d));
        sphere.GetComponent<MeshRenderer>().material.color = colorOrig;
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(target.position, player.position);

        UpdateTransparency(distance);

        if (distance < (radius + 0.3f) && !soundPlayed)
        {
            AudioSource.PlayClipAtPoint(audio, target.position); 
            soundPlayed = true;

            foreach (GameObject obj in highlights) 
            {
                obj.SetActive(true);
            }
            //Debug.Log("Audio played");

            transformList.Add(new Vector2(target.position.x - initPosition.x, target.position.z - initPosition.z));
            StaticData.transforms = ListToText(transformList);
        }

        
    }

    void RecordTransform()
    {
        Vector2 currPlayer = new Vector2(player.position.x - initPosition.x, player.position.z - initPosition.z);
        transformList.Add(currPlayer);
    }

    string ListToText(List<Vector2> list)
    {
        string result = "";

        foreach(var x in list)
        {
            result += "(" + x.x.ToString() + "; " + x.y.ToString() + ") ";
        }

        return result;
    }  
}
