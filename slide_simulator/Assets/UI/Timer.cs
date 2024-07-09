using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0f;
    public bool timeRunning = true;
    public TMP_Text timer2;
    // Start is called before the first frame update
    void Start()
    {
        timeRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }

    void DisplayTime(float ShowTime)
    {
        ShowTime += 1;
        float minutes = Mathf.FloorToInt(ShowTime / 60);
        float seconds = Mathf.FloorToInt(ShowTime % 60);
        timer2.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
   

}
