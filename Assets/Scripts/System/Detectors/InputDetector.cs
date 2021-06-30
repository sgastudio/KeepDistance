using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
public class InputDetector : CollisionDetector, IPunObservable
{
    // Start is called before the first frame update
    [Header("Input")]
    public float delay = 0.2f;
    public string axisName;
    public UnityEvent inputingEvent;
    [ROA]
    public bool isInputing;
    [ROA]
    public bool inputCooldown = false;
    float LastInteractTime;


    public override void Start()
    {
        base.Start();
    }

    public virtual void Update()
    {
        UpdateList();
        if (!string.IsNullOrWhiteSpace(axisName))
            isInputing = Input.GetAxis(axisName) > 0 && inputCooldown == false;
        else
            Debug.LogWarning("Please check the axis name on " + gameObject.ToString());
        firing();

        if (Time.time > LastInteractTime + delay)
        {
            inputCooldown = false;
        }
    }

    void UpdateList()
    {
        if (activeList.Count > 1)
            activeList.Sort(compareDistance);
    }

    void activateFiring()
    {
        inputCooldown = true;
        LastInteractTime = Time.time;
    }

    void firing()
    {
        if (isInputing && activeList.Count > 0 && GetNetworkingTest())
        {
            activateFiring();
            inputingEvent.Invoke();
        }
    }

    void syncedFiring()
    {
        if (isInputing && activeList.Count > 0)
        {
            activateFiring();
            inputingEvent.Invoke();
        }
    }

    int compareDistance(GameObject x, GameObject y)
    {
        float dstx = Vector3.Distance(this.transform.position, x.transform.position);
        float dsty = Vector3.Distance(this.transform.position, y.transform.position);

        if (dstx > dsty)
            return 1;
        else if (dstx == dsty)
            return 0;
        else
            return -1;

        //return (int)(dstx - dsty); //inaccuray
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //TODO: might be slow to control in this way

        if (stream.IsWriting)
        {
            stream.SendNext(isInputing || inputCooldown);
        }
        else
        {
            this.isInputing = (bool)stream.ReceiveNext();
            syncedFiring();
        }
    }
}
