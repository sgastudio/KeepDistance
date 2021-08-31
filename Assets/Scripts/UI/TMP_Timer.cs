using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMP_Timer : MonoBehaviour
{
    NetworkManager networkManager;
    TMP_Text textElement;
    public Timer timer;
    public string prefix = "Time: ";
    public string suffix;
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
                    textElement.text = prefix + "Paused" + suffix;
                    break;
                case Timer.TimerState.Running:
                    int minutes = ((int)(timer.duration - timer.passedTime) / 60);
                    int seconds = ((int)(timer.duration - timer.passedTime) % 60);
                    textElement.text = prefix + minutes.ToString() + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString()) + suffix;
                    break;
                case Timer.TimerState.Finished:
                    textElement.text = prefix + "End" + suffix;
                    break;
                default:
                    textElement.text = prefix + "Waiting" + suffix;
                    break;
            }

        }
    }
}
