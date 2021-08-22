using System.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
public class ForceFieldDetector : CollisionDetector
{
    // Start is called before the first frame update
    [Header("Force")]
    public Vector3 force;

    public Vector3 torque;

    public ForceMode forceMode;

    public override void Start()
    {
        base.Start();
        targetEnter.AddListener(ApplyForce);
    }

    public void ApplyForce(Collider other)
    {
        Rigidbody body = other.attachedRigidbody;
        if (body)
        {
            body.AddForce(force, forceMode);
            body.AddTorque(torque, forceMode);
        }
    }
}
