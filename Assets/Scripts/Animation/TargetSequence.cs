using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformSequence
{
    public TransformSequence(Transform trans)
    {
        this.transfrom = trans;
    }
    public Transform transfrom;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
}

public class TargetSequence : MonoBehaviour
{
    public List<TransformSequence> transformList;
    public float duration = 2f;
    public bool playOnStart;
    public bool useFixedUpdate = true;
    public bool autoNext = false;
    public bool autoLast = false;

    bool isPlaying;
    bool isRotating;
    bool isReverse;
    float percentage;

    [SerializeField,ReadOnlyAttribute(true)]
    int currentIndex;
    [SerializeField,ReadOnlyAttribute(true)]
    int targetIndex;
    [SerializeField,ReadOnlyAttribute(true)]
    Vector3 initialPosition;
    [SerializeField,ReadOnlyAttribute(true)]
    Quaternion initialRotation;
    [SerializeField,ReadOnlyAttribute(true)]
    Vector3 targetPosition;
    [SerializeField,ReadOnlyAttribute(true)]
    Quaternion targetRotation;
    Rigidbody body = default;

    void Start()
    {
        transformList.Insert(0, new TransformSequence(this.transform));
        currentIndex = 0;

        if (transformList.Count > 1)
            targetIndex = 1;

        changePositionAndRotation();

        if (playOnStart)
            isPlaying = true;

        if (!body)
            body = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!useFixedUpdate && (isPlaying || isRotating) && transformList.Count > 1)
            Animation();
    }

    void FixedUpdate()
    {
        if (useFixedUpdate && (isPlaying || isRotating) && transformList.Count > 1)
            Animation();
    }

    void Animation()
    {
        float delta = Time.deltaTime / duration;

        if (isReverse)
            percentage = percentage - delta;
        else
            percentage = percentage + delta;


        if (percentage >= 1.0f)
        {
            percentage = 1.0f;
            // if(currentIndex+1<targetIndex)
            //     Warp
            // else if (autoNext)
            if (autoNext)
                Next();
            else if (isPlaying)
            {
                isPlaying = false;
                isRotating = true;
                percentage = 0f;
            }
            else if (isRotating)
            {
                isRotating = false;
            }
            // if (autoReverse)
            //     isReverse = true;
            // else
            //     isPlaying = false;

        }
        else if (percentage <= 0.0f)
        {
            percentage = 0.0f;
            if (autoLast)
                Last();
            else if (isPlaying)
            {
                isPlaying = false;
                isRotating = true;
                percentage = 1f;
            }
            else if (isRotating)
            {
                isRotating = false;
            }
            // if (autoReverse)
            //     isReverse = false;
            // else
            //     isPlaying = false;
        }

        if (body)
        {
            if (isRotating)
                //this.body.MoveRotation(Quaternion.LerpUnclamped(this.body.rotation, targetRotation, percentage));
                this.body.rotation = Quaternion.Lerp(this.body.rotation, targetRotation, percentage);
            else if(isPlaying)
            {
                if(targetPosition - this.body.position != Vector3.zero)
                this.body.rotation = Quaternion.Lerp(this.body.rotation, Quaternion.LookRotation(targetPosition - this.body.position), 0.5f);
                this.body.MovePosition(Vector3.LerpUnclamped(initialPosition, targetPosition, percentage));
            }
        }
        else
        {
            if (isRotating)
                this.transform.rotation = this.transform.rotation * Quaternion.LerpUnclamped(initialRotation, targetRotation, percentage);
            else if(isPlaying)
                this.transform.Translate(Vector3.LerpUnclamped(initialPosition, targetPosition, percentage));

        }
    }

    void changePositionAndRotation()
    {
        Transform current, target;
        current = transformList[currentIndex].transfrom;
        target = transformList[targetIndex].transfrom;

        this.initialPosition = transformList[currentIndex].position;
        this.initialRotation = transformList[currentIndex].rotation;
        this.targetPosition = transformList[targetIndex].position;
        this.targetRotation = transformList[targetIndex].rotation;
        if (current)
        {
            this.initialPosition += current.position;
            this.initialRotation *= current.rotation;
        }

        if (target)
        {
            this.targetPosition += target.position;
            this.targetRotation *= target.rotation;
        }
    }

    public void Play(bool playing = true)
    {
        this.isPlaying = playing;
        this.isRotating = false;
    }

    public void Reverse(bool reverse = true)
    {
        this.isReverse = reverse;
    }

    public void Next()
    {
        if (targetIndex < transformList.Count - 1)
        {
            targetIndex += 1;
            currentIndex += 1;
            changePositionAndRotation();
            Reverse(false);
            Play();
            percentage = 0f;
        }
    }

    public void Last()
    {
        if (currentIndex >= 1)
        {
            targetIndex -= 1;
            currentIndex -= 1;
            changePositionAndRotation();
            Reverse();
            Play();
            percentage = 1f;
        }
    }

    // public void Warp(int i)
    // {

    // }

    // public void Sequence(int i)
    // {

    // }
}
