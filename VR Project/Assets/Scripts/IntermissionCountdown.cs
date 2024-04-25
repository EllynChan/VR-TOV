using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntermissionCountdown : MonoBehaviour
{
    float countdown = 30;

    // Update is called once per frame
    void Update()
    {
        if (countdown > 1)
        {
            countdown -= Time.deltaTime;
            this.GetComponent<TMP_Text>().text = Mathf.FloorToInt(countdown).ToString();
        }
    }
}
