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
 * Changes the object shown upon clicking "Button"
 * Handles the scoring of objects and generates csv with 
 * parameters Object (object name),Score,Time (world time when rating is complete)
**/

//class used to store information for the csv
public class KeyFrame
{
    string _name;
    float _score;
    float _time;

    public KeyFrame (string name, float score, float time)
    {
        _name = name;
        _score = score;
        _time = time;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public float Score
    {
        get => _score;
        set => _score = value;
    }

    public float Time
    {
        get => _time;
        set => _time = value;
    }
}

public class ChangeItem : MonoBehaviour
{
    public GameObject[] CSToActivate;
    public GameObject[] USToActivate;
    public GameObject firstObject;
    public GameObject CS;
    public SteamVR_LaserPointer laserPointer;
    public GameObject slider;
    public GameObject numberText;
    public GameObject thisPanel;
    public GameObject finishingPanel;

    private Stack<int> CSIndices;
    private Stack<int> USIndices;
    private List<KeyFrame> keyFrames = new List<KeyFrame>(100);
    private string singleObjectName = "Bamboo";
    private float time = 0;
    private float _score;
    public float Score
    {
        get => _score;
        set
        {
            _score = value;
            if (_score != 0f)
                keyFrames.Add(new KeyFrame (singleObjectName, value, Time.time - time));
        }
    }

    void Start()
    {
        FillObjectIndices();
        laserPointer.PointerClick += PointerClick;
        Score = 0f;
    }
    
    void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button" && CSIndices.Count != 0)
        {
            ResetPanel();
            ShowCSObject(); 
        } else {
            ResetPanel();
            ShowUSObject();
        }
    }

    void ShowCSObject()
    {
        int index = CSIndices.Pop();
        for (int i = 0; i < CSToActivate.Length; i++)
        {
            CSToActivate[i].SetActive(i == index);

            if (CSToActivate[i].activeSelf)
                singleObjectName = CSToActivate[i].name;
        }
    }

    void ShowUSObject()
    {
        if (CS.activeSelf)
            CS.SetActive(false);

        if (USIndices == null || USIndices.Count == 0)
        {
            Score = slider.GetComponent<Slider>().value;
            Finish();
            return;
        }
        int index = USIndices.Pop();
        for (int i = 0; i < USToActivate.Length; i++)
        {
            USToActivate[i].SetActive(i == index);

            if (USToActivate[i].activeSelf)
                singleObjectName = USToActivate[i].name;
        }
    }
 
    void FillObjectIndices()
    {
        System.Random random1 = new System.Random();
        System.Random random2 = new System.Random();
        CSIndices = new Stack<int>();
        USIndices = new Stack<int>();
        for (int i = 0; i < CSToActivate.Length; ++i)
            CSIndices.Push(i);
        CSIndices = new Stack<int>(CSIndices.OrderBy(x => random1.Next()));
        for (int i = 0; i < USToActivate.Length; ++i)
            USIndices.Push(i);
        USIndices = new Stack<int>(USIndices.OrderBy(x => random2.Next()));
    }

    void ResetPanel()
    {
        Score = slider.GetComponent<Slider>().value;
        numberText.GetComponent<TMP_Text>().text = "Please make a rating";
        slider.GetComponent<Slider>().value = 5.5f;
        Score = 0f;
        time = Time.time;
        
        if (firstObject.activeSelf)
            firstObject.SetActive(false);
    }

    string ToCSV()
    {
        var sb = new StringBuilder("Object,Score,Time");
        foreach(var frame in keyFrames)
        {
            sb.Append('\n').Append(frame.Name.ToString()).Append(',').Append(frame.Score.ToString())
            .Append(',').Append(frame.Time.ToString());
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
        // to know the exact file path of this destination use the following:
        // Debug.Log(Application.persistentDataPath);
        var folder = Application.persistentDataPath;
    #endif

        var filePath = Path.Combine(folder, System.DateTime.Now.ToString("yyyy-MM-ddTHH.mm.ss") + "TransferOfValence.csv");

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

    void Finish()
    {
        thisPanel.SetActive(false);
        finishingPanel.SetActive(true);
        keyFrames.RemoveAt(0);
        keyFrames.RemoveAt(keyFrames.Count - 1);
        SaveToFile();
    }
}
