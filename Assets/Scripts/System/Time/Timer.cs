using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Time Options")]
    public float duration = 10f;
    [ROA]
    public float passedTime;
    public bool activeOnStart = false;
    public bool unscaled = true;
    [Header("Events")]
    public UnityEvent onStart;
    public UnityEvent onEnd;
    public UnityEvent onPaused;
    public UnityEvent onResume;

    [Header("UI")]
    public Text timerText;

    public enum TimerState
    {
        Initial,
        Stopped,
        Finished,
        Paused,
        Running,
    }
    TimerState state = TimerState.Initial;

    // Start is called before the first frame update
    void Start()
    {
        passedTime = 0;
        if (activeOnStart)
            StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == TimerState.Running)
            if (unscaled)
                passedTime += Time.unscaledDeltaTime;
            else
                passedTime += Time.deltaTime;

        if (passedTime >= duration)
            StopTimer();

        if (timerText)
        {
            switch (state)
            {
                case TimerState.Paused:
                    timerText.text = "Paused";
                    break;
                case TimerState.Running:
                    timerText.text = "Time Remain: " + ((int)(duration - passedTime) / 60).ToString() + ":" + ((int)(duration - passedTime) % 60).ToString();
                    break;
                default:
                    timerText.text = "Waiting";
                    break;
            }
        }
    }

    public void StartTimer()
    {
        if (state != TimerState.Running)
        {
            if (state == TimerState.Paused)
                onResume.Invoke();
            else
                onStart.Invoke();
            state = TimerState.Running;
        }
    }

    public void PauseTimer()
    {
        if (state == TimerState.Running)
        {
            onPaused.Invoke();
            state = TimerState.Paused;
        }
    }

    public void StopTimer()
    {
        if (state < TimerState.Paused)
            return;

        if (passedTime < duration)
            state = TimerState.Stopped;
        else
            state = TimerState.Finished;
        passedTime = 0;
        onEnd.Invoke();
    }

    public TimerState GetTimerState()
    {
        return state;
    }
}
