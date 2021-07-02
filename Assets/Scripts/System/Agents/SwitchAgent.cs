using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class SwitchAgent : MonoBehaviourPun
{
    public string switchName;
    public bool state;
    public UnityEvent onStateChanged;
    public UnityEvent onStateClose;
    public UnityEvent onStateOpen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void SwitchState(bool nState)
    {
        if(this.state == nState)
            return;
        this.state = nState;
        if(this.state)
            onStateOpen.Invoke();
        else
            onStateClose.Invoke();
        onStateChanged.Invoke();
    }

    public void SwitchOnce()
    {
        //SwitchState(!this.state);
        photonView.RPC("SwitchState",RpcTarget.All, !this.state);
    }
}
