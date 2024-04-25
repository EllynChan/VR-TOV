using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

/**
 * Stored global data for the scene handler in transition scene.
 * On load of each scene the index will be updated to its build index to 
 * ensure that the scenes load in the correct order.
 * 
 * Stores the name of the last scene and the two objects and the walking time
 * in the last scene so that the data are kept upon loading the transition scene.
 * 
 * Stores the list of Experiment Parameters and pause/finish information so that they are kept
 * upon scene change.
 **/

 public class StaticData : MonoBehaviour 
 {
   // index keeps track of the number of times the scene blocks repeated
    public static int index = 0;

    //these are data that the encoding csv file needs
    public static string scene = "";
    public static string objectOne = "";
    public static string objectTwo = "";
    public static float timeToCircle = 0;
    public static string transforms = "";

    //indicates when the encoding phase is finished and when to generate the encoding csv
    public static bool hasFinished = false;

    //the stack that stores the order of scenes in each block
    public static Stack<int> scenesIndices;

    //the list of csv inputs, adds one entry (row) for every encoding scene
    public static List<ExperimentParameter> parameters = new List<ExperimentParameter>(100);

    public static bool gamePaused = false;
    public static bool intermission = false;

    //variables needed to decide the order of sets for this run
    private static int numberOfScenes = 10;
    private static int numberOfRepeats = 4;
    public static int[,] array2D = new int[numberOfScenes, numberOfRepeats];

    void Awake()
    {
        DontDestroyOnLoad(this);
        GenerateSetSequence();
    }

    void GenerateSetSequence()
    {
      System.Random random = new System.Random();
      for (int i = 0; i < numberOfScenes; i++)
      {
         Queue<int> setNumbers = new Queue<int>();
         for (int j = 0; j < numberOfRepeats; j++)
            setNumbers.Enqueue(j);
         setNumbers = new Queue<int>(setNumbers.OrderBy(x => random.Next()));
         for (int k = 0; k < numberOfRepeats; k++)
            array2D[i,k] = setNumbers.Dequeue();
      }
    }
}