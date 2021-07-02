using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
public class CollisionStateSyncAgent : MonoBehaviour,IPunObservable
{
    Collider m_collider;
    void Start()
    {
        m_collider = this.GetComponent<Collider>();
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
            stream.SendNext(m_collider.enabled);
        }
        else
        {
            m_collider.enabled = (bool)stream.ReceiveNext();
        }
    }


    #endregion
}
