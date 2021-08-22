using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class SelfAnimation : MonoBehaviour
{
    public bool overrideInitialState = false;

    public bool enablePosition;
    [ShowIf("@this.overrideInitialState && this.enablePosition")]
    public Vector3 overridePosition;
    [ShowIf("enablePosition")]
    public Vector3 targetPosition = default;

    public bool enableRotation;
    [ShowIf("@this.overrideInitialState && this.enableRotation")]
    public Vector3 overrideRotation;
    [ShowIf("enableRotation")]
    public Vector3 targetRotation = default;


    public float duration = 2f;
    public bool isRelativeTranslation;

    public bool useFixedUpdate = true;
    public bool playOnStart;
    public bool autoReverse;

    public UnityEvent onAnimationEnd;

    bool isReverse;
    bool isPlaying;
    float percentage;
    Vector3 initialPosition;
    Quaternion initialRotation;

    [SerializeField]
    Rigidbody body = default;

    void Start()
    {
        if (overrideInitialState)
        {
            initialPosition = overridePosition;
            initialRotation = Quaternion.Euler(overrideRotation);
        }
        else
        {
            initialPosition = this.transform.position;
            initialRotation = this.transform.rotation;
        }

        if (isRelativeTranslation)
        {
            targetPosition = initialPosition + targetPosition;
            targetRotation = (initialRotation * Quaternion.Euler(targetRotation)).eulerAngles;
        }

        if (playOnStart)
            isPlaying = true;

        if(!body)
            Debug.LogWarning("Animation script using none rigibody transformation");

    }

    void Update()
    {
        if (!useFixedUpdate && isPlaying)
            Transformation();
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate && isPlaying)
            Transformation();
    }

    void Transformation()
    {
        float delta = Time.deltaTime / duration;
        if (isReverse)
            percentage = percentage - delta;
        else
            percentage = percentage + delta;


        if (percentage >= 1.0f)
        {
            percentage = 1.0f;
            if (autoReverse)
                isReverse = true;
            else
                Stop();
        }
        else if (percentage <= 0.0f)
        {
            percentage = 0.0f;
            if (autoReverse)
                isReverse = false;
            else
                Stop();
        }

        if (body)
        {
            if (enablePosition)
                this.body.MovePosition(Vector3.LerpUnclamped(initialPosition, targetPosition, percentage));
            if (enableRotation)
                this.body.MoveRotation(Quaternion.LerpUnclamped(initialRotation, Quaternion.Euler(targetRotation), percentage));
        }
        else
        {
            if (enablePosition)
                this.transform.position = (Vector3.LerpUnclamped(initialPosition, targetPosition, percentage));
            if (enableRotation)
                this.transform.rotation = this.transform.rotation * Quaternion.LerpUnclamped(initialRotation, Quaternion.Euler(targetRotation), percentage);
        }

    }

    public void Play()
    {
        isPlaying = true;
    }

    public void Reverse(bool State)
    {
        isReverse = State;
    }

    public void SetReverseAndPlay(bool State)
    {
        Reverse(State);
        Play();
    }

    public void Stop()
    {
        isPlaying = false;
        onAnimationEnd.Invoke();
    }
}
