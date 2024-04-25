using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformBlockNum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int blockNum = StaticData.index + 1;
        this.GetComponent<TMP_Text>().text = "You are now entering block " + blockNum.ToString();
    }
    
}
