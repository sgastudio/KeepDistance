using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon;
using Photon.Pun;

public class Timer : MonoBehaviour, IPunObservable
{
    [Header("Time Options")]
    public float duration = 10f;
    [ROA]
    public float passedTime;
    public bool activeOnStart = false;
    public bool unscaled = true;
    public bool synchronization = false;
    
    [Header("Events")]
    public bool masterOnly = false;
    public UnityEvent onStart;
    public UnityEvent onEnd;
    public UnityEvent onPaused;
    public UnityEvent onResume;

    public enum TimerState
    {
        Initial,
        Stopped,
        Finished,
        Paused,
        Running,
    }
    [ROA]
    public TimerState state = TimerState.Initial;

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
    }

    public void StartTimer()
    {
        if (state != TimerState.Running)
        {
            state = TimerState.Running;
            if(masterOnly && !PhotonNetwork.IsMasterClient)
                return;
            if (state == TimerState.Paused)
                onResume.Invoke();
            else
                onStart.Invoke();
        }
    }

    public void PauseTimer()
    {
        if (state == TimerState.Running)
        {
            state = TimerState.Paused;
            if(masterOnly && !PhotonNetwork.IsMasterClient)
                return;
            onPaused.Invoke();
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

        if(masterOnly && !PhotonNetwork.IsMasterClient)
                return;
        onEnd.Invoke();
    }

    public TimerState GetTimerState()
    {
        return state;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (synchronization)
            if (stream.IsWriting)
            {
                stream.SendNext(passedTime);
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                passedTime = (float)stream.ReceiveNext() + lag;
            }
    }
}
