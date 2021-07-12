using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class FootIK : MonoBehaviour
{
    Animator pAnimator;

    public Transform AgentLFootTransform;
    public Transform AgentRFootTransform;

    public Vector2 IKLFootWeight;
    public Vector2 IKRFootWeight;

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
            pAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKLFootWeight.x);
            pAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKRFootWeight.x);
            //rotation weight
            pAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IKLFootWeight.y);
            pAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, IKRFootWeight.y);
            //IK position
            pAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, AgentLFootTransform.position);
            pAnimator.SetIKPosition(AvatarIKGoal.RightFoot, AgentRFootTransform.position);
            //IK rotation
            pAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, AgentLFootTransform.rotation);
            pAnimator.SetIKRotation(AvatarIKGoal.RightFoot, AgentRFootTransform.rotation);
        }
    }
}
