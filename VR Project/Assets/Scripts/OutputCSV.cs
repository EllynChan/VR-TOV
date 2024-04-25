using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using System;
using System.Text;
using System.Linq; 
using System.IO;
using TMPro;

#if UNITY_EDITOR
    using UnityEditor;
#endif

/**
 * Handles outputing csv for the Encoding phase.
 * 
 * An entry will be added to the list of Experiment Parameters in StaticData.parameters every
 * time Score is changed, with the current scene name, object names in the scene, the time
 * it took to reach the circle, and the time it took to make the rating
 **/


// data that will be collected
public class ExperimentParameter
{
    string _scene;
    string _condition;
    string _inOrOut;
    string _complexity;
    string _objectOne;
    string _objectTwo;
    string _movementPoints;
    float _walkingTime;
    float _score;
    float _scoreTime;

    public ExperimentParameter (string scene, string condition, string inOrOut, string complexity, string objectOne, 
        string objectTwo, string movementPoints, float walkingTime, float score, float scoreTime)
    {
        _scene = scene;
        _condition = condition;
        _inOrOut = inOrOut;
        _complexity = complexity;
        _objectOne = objectOne;
        _objectTwo = objectTwo;
        _movementPoints = movementPoints;
        _walkingTime = walkingTime;
        _score = score;
        _scoreTime = scoreTime;
    }

    public string Scene
    {
        get => _scene;
        set => _scene = value;
    }

    public string Condition
    {
        get => _condition;
        set => _condition = value;
    }

    public string InOrOut
    {
        get => _inOrOut;
        set => _inOrOut = value;
    }

    public string Complexity
    {
        get => _complexity;
        set => _complexity = value;
    }

    public string ObjectOne
    {
        get => _objectOne;
        set => _objectOne = value;
    }

    public string ObjectTwo
    {
        get => _objectTwo;
        set => _objectTwo = value;
    }

    public string MovementPoints
    {
        get => _movementPoints;
        set => _movementPoints = value;
    }

    public float WalkingTime
    {
        get => _walkingTime;
        set => _walkingTime = value;
    }

    public float Score
    {
        get => _score;
        set => _score = value;
    }

    public float ScoreTime
    {
        get => _scoreTime;
        set => _scoreTime = value;
    }
}

public class OutputCSV : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    public GameObject numberText;
    public GameObject slider;

    private string cond;
    private string inout;
    private string comp;
    private bool saved = false;
    private float startTime;
    private float _score;
    public float Score
    {
        get => _score;
        set
        {
            _score = value;
            if (_score != 0f)
            {
                DecideConditions();
                StaticData.parameters.Add(new ExperimentParameter 
                    (StaticData.scene, cond, inout, comp, StaticData.objectOne,
                        StaticData.objectTwo, StaticData.transforms, StaticData.timeToCircle, value, Time.time - startTime));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        laserPointer.PointerClick += PointerClick;
        cond = "";
        Score = 0f;
        startTime = Time.time;
    }

    void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button")
        {
            Score = slider.GetComponent<Slider>().value;
            Score = 0f;
        }
    }

    void DecideConditions()
    {
        if (StaticData.scene == "Desert" || StaticData.scene == "Lab" ||
            StaticData.scene == "SeaPort" || StaticData.scene == "Library" ||
            StaticData.scene == "Hospital")
        {
            cond = "Negative";
        } else {
            cond = "Neutral";
        }

        if (StaticData.scene == "Desert" || StaticData.scene == "Park" ||
            StaticData.scene == "Forest" || StaticData.scene == "SeaPort")
        {
            inout = "Outdoors";
        } else {
            inout = "Indoors";
        }

        if (StaticData.scene == "Forest" || StaticData.scene == "SeaPort" ||
            StaticData.scene == "Library" || StaticData.scene == "LivingRoom")
        {
            comp = "Complex";
        } else {
            comp = "Simple";
        }
    }

    string ToCSV()
    {
        StaticData.parameters.RemoveAt(0);
        var sb = new StringBuilder("Scene,Scene Condition,Indoors/Outdoors,Scene Complexity,Ohject 1,Object 2,Movement Points,Walking Time,Rating,Rating Time");
        foreach(var p in StaticData.parameters)
        {
            sb.Append('\n').Append(p.Scene.ToString()).Append(',')
            .Append(p.Condition.ToString()).Append(',').Append(p.InOrOut.ToString()).Append(',')
            .Append(p.Complexity.ToString()).Append(',').Append(p.ObjectOne.ToString()).Append(',')
            .Append(p.ObjectTwo.ToString()).Append(',').Append(p.MovementPoints.ToString()).Append(',')
            .Append(p.WalkingTime.ToString()).Append(',').Append(p.Score.ToString()).Append(',')
            .Append(p.ScoreTime.ToString());
        }

        return sb.ToString();
    }

    void SaveToFile ()
    {
        // Use the CSV generation from before
        var content = ToCSV();

        // The target file path e.g.
    #if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if(! Directory.Exists(folder)) Directory.CreateDirectory(folder);
    #else
        var folder = Application.persistentDataPath;
    #endif

        var filePath = Path.Combine(folder, System.DateTime.Now.ToString("yyyy-MM-ddTHH.mm.ss") + "Encoding.csv");

        using(var writer = new StreamWriter(filePath, false))
        {
            writer.Write(content);
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"CSV file written to \"{filePath}\"");

    #if UNITY_EDITOR
        AssetDatabase.Refresh();
    #endif
    }

    void Update() 
    {
        if (StaticData.hasFinished && !saved)
        {
            SaveToFile();  
            saved = true;
        }
    }
}
