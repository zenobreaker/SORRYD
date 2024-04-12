using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI; 

public class RemainTimer : MonoBehaviour
{
    Text text;
    TimeSpan timeSpan;

    // Start is called before the first frame update
    void Awake()
    {
        
        text = GetComponent<Text>(); 
    }

    public void SetTime()
    {
        int totalTime = (int)GameManager.instance.GetRoundRemainTime();
        timeSpan = new TimeSpan(totalTime / 3600, totalTime / 60, totalTime % 60);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetTime(); 
        text.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
