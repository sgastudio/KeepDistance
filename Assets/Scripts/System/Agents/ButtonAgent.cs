using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
[RequireComponent(typeof(PhotonView))]
public class ButtonAgent : MonoBehaviourPun, IInteractable
{
    public string switchName;
    public bool state;
    public float resetTime=2.0f;
    public UnityEvent onStateChanged;
    public UnityEvent onStateClose;
    public UnityEvent onStateOpen;
    float lastInteract;

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
        if (PhotonNetwork.OfflineMode)
            SwitchState(true);
        else
            photonView.RPC("SwitchState", RpcTarget.All, true);
        lastInteract = Time.time;
        if(resetTime>0)
        {
            StopAllCoroutines();
            StartCoroutine(Reset());
        }
    }

    public void Interact()
    {
        SwitchOnce();
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);
        if (PhotonNetwork.OfflineMode)
            SwitchState(false);
        else
            photonView.RPC("SwitchState", RpcTarget.All, false);
    }
}
