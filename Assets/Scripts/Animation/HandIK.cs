using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    Animator pAnimator;

    public Transform AgentLHandTransform;
    public Transform AgentRHandTransform;

    public Vector2 IKLHandWeight;
    public Vector2 IKRHandWeight;

    Transform currentLHandTransform;
    Transform currentRHandTransform;

    // Start is called before the first frame update
    void Start()
    {
        pAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (pAnimator != null)
        {
            //position weight
            pAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKLHandWeight.x);
            pAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKRHandWeight.x);
            //rotation weight
            pAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKLHandWeight.y);
            pAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKRHandWeight.y);

            pAnimator.SetIKPosition(AvatarIKGoal.LeftHand, AgentLHandTransform.position);
            pAnimator.SetIKPosition(AvatarIKGoal.RightHand, AgentRHandTransform.position);

            pAnimator.SetIKRotation(AvatarIKGoal.LeftHand, AgentLHandTransform.rotation);
            pAnimator.SetIKRotation(AvatarIKGoal.RightHand, AgentRHandTransform.rotation);
        }
    }
}

 
