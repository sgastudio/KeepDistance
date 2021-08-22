using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMP_Timer : MonoBehaviour
{
    NetworkManager networkManager;
    TMP_Text textElement;
    public Timer timer;
    void Awake()
    {
        textElement = this.GetComponent<TMP_Text>();
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
                    int minutes = ((int)(timer.duration - timer.passedTime) / 60);
                    int seconds = ((int)(timer.duration - timer.passedTime) % 60);
                    textElement.text = "Time: " + minutes.ToString() + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
                    break;
                case Timer.TimerState.Finished:
                    textElement.text = "Time: End";
                    break;
                default:
                    textElement.text = "Time: Waiting";
                    break;
            }

        }
    }
}
