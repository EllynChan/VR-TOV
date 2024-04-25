using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using TMPro;

public class SliderInput : MonoBehaviour
{
    public SteamVR_Action_Vector2 rating;
    public GameObject nextButton;
    public GameObject ratingValue;

    private Slider slider;

    void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    void Update()
    {

        nextButton.SetActive(ratingValue.GetComponent<TMP_Text>().text.Length <= 2);

        if(rating.axis.magnitude > 0.1f)
        {
            if((rating.axis.y + rating.axis.x) > 0f) 
                slider.value += 0.05f;
            else 
                slider.value -= 0.05f;   
            
            ratingValue.GetComponent<TMP_Text>().text = Mathf.Floor(slider.value).ToString();
        }
    }
}
