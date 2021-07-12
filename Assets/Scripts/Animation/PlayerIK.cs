using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerIK : MonoBehaviour
{
    Animator pAnimator;
    public IKGoalEntry[] playerIKGoal;
    public IKHintEntry[] playerIKHint;

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
            foreach (IKGoalEntry i in playerIKGoal)
            {
                i.UpdateWeight(pAnimator);
            }

            foreach (IKHintEntry i in playerIKHint)
            {
                i.UpdateWeight(pAnimator);
            }
        }
    }
}

[Serializable]
public class IKGoalEntry
{
    public AvatarIKGoal iKGoal;
    public GameObject attachPoint;
    public Vector2 weight;
    public void UpdateWeight(Animator ani)
    {
        //update IK Goal weight x->position y->rotation
        if (ani.GetIKPositionWeight(iKGoal) != weight.x)
            ani.SetIKPositionWeight(iKGoal, weight.x);

        if (ani.GetIKRotationWeight(iKGoal) != weight.y)
            ani.SetIKRotationWeight(iKGoal, weight.y);

        //update IK Goal transform
        if (attachPoint == null || weight.magnitude==0)
            return;
        ani.SetIKPosition(iKGoal, attachPoint.transform.position);
        ani.SetIKRotation(iKGoal, attachPoint.transform.rotation);
    }
}

[Serializable]
public class IKHintEntry
{
    public AvatarIKHint iKHint;
    public GameObject attachPoint;
    public float weight;
    public void UpdateWeight(Animator ani)
    {
        //update IK weight x->position y->rotation
        if (ani.GetIKHintPositionWeight(iKHint) != weight)
            ani.SetIKHintPositionWeight(iKHint, weight);


        //update IK Hint transform
        if (attachPoint != null && weight!=0)
            ani.SetIKHintPosition(iKHint, attachPoint.transform.position);
    }
}