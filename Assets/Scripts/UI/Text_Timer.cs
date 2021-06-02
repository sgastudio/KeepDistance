using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Timer : MonoBehaviour
{
    Text textElement;
    public Timer timer;
    void Awake()
    {
        textElement = this.GetComponent<Text>();
        PrintText();
    }

    // Update is called once per frame
    void Update()
    {
        PrintText();
    }

    void PrintText()
    {
        if (textElement && timer)
        {
            switch (timer.state)
            {
                case Timer.TimerState.Paused:
                    textElement.text = "Time: Paused";
                    break;
                case Timer.TimerState.Running:
                    textElement.text = "Time: " + ((int)(timer.duration - timer.passedTime) / 60).ToString() + ":" + ((int)(timer.duration - timer.passedTime) % 60).ToString();
                    break;
                default:
                    textElement.text = "Time: Waiting";
                    break;
            }

        }
    }
}
