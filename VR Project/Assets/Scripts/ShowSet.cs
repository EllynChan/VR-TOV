using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowSet : MonoBehaviour
{
    public GameObject[] sets;

    // Start is called before the first frame update
    void Start()
    {
        ChooseSet(DecideSceneIndex());
    }

    int DecideSceneIndex()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Desert":
                return 0;
            case "Lab":
                return 1;
            case "Kitchen":
                return 2;
            case "Park":
                return 3;
            case "Office":
                return 4;
            case "Forest":
                return 5;
            case "SeaPort":
                return 6;
            case "Library":
                return 7;
            case "LivingRoom":
                return 8;
            case "Hospital":
                return 9;
            default:
                return -1;
        }
    }

    void ChooseSet(int i)
    {
        for (int j = 0; j < sets.Length - 1; j++)
        {
            sets[j].SetActive(false);
        }

        sets[StaticData.array2D[i, StaticData.index - 1]].SetActive(true);
    }
}
