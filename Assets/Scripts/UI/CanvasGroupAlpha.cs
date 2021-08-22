using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupAlpha : MonoBehaviour
{
    public float waitForseconds;
    public float duration = 1f;
    public bool playOnStart = true;

    [Range(0f, 1f)]
    public float initialAlpha = 0.0f;
    [Range(0f, 1f)]
    public float targetAlpha = 1.0f;

    bool playing;
    float startTime;
    CanvasGroup group;
    // Start is called before the first frame update
    void Start()
    {
        group = this.GetComponent<CanvasGroup>();

        if (playOnStart)
            Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing && Time.time > startTime + waitForseconds)
            group.alpha = Mathf.MoveTowards(group.alpha, targetAlpha, Time.deltaTime / duration);
    }

    public void Play()
    {
        playing = true;
        startTime = Time.time;
        group.alpha = initialAlpha;
    }

    public void Stop()
    {
        playing = false;
    }
}
