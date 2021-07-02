using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
public class KinematicStateSyncAgent : MonoBehaviour,IPunObservable
{
    Rigidbody m_body;
    void Start()
    {
        m_body = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

  #region IPunObservable implementation

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //TODO: might be slow to control in this way
        if (stream.IsWriting)
        {
            stream.SendNext(m_body.isKinematic);
        }
        else
        {
            m_body.isKinematic = (bool)stream.ReceiveNext();
        }
    }


    #endregion
}
