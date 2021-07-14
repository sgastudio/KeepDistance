using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfLookAt : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset = Vector3.up * 1.5f;

    public bool useSlerp = false;

    public float slerpWeight = 0.5f;

    void Update()
    {

        if (target)
        {

            Quaternion tQ = Quaternion.LookRotation((target.position + targetOffset - this.transform.position).normalized);
            RotationCaculation(tQ);
        }
        else
        {
            Quaternion tQ = Quaternion.LookRotation(this.transform.parent.forward);
            RotationCaculation(tQ);
        }

    }

    void RotationCaculation(Quaternion tQ)
    {
        if (useSlerp)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, tQ, slerpWeight);
        }
        else
        {
            if (target)
                this.transform.LookAt(target);
            else
                this.transform.localRotation = Quaternion.identity;
        }
    }

    public void SetTarget(Transform trans)
    {
        target = trans;
    }

    public void SetTarget(Collider other)
    {
        SetTarget(other.transform);
    }

    public void RemoveTarget()
    {
        target = null;
    }
}
