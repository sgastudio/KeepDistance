using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerAgent : MonoBehaviourPun
{
    public string playerName;
    public int playerGroup;

    public ParticleSystem virusParticle;
    public ParticleSystem cleanParticle;

    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(playerName))
            playerName = PhotonNetwork.LocalPlayer.NickName;
        BindCamera();
    }

    void BindCamera()
    {
        CameraOperation camera = Camera.main.GetComponent<CameraOperation>();

        if (camera)
        {
            if (photonView.IsMine)
                camera.TrackingObject(this.gameObject);
        }
        else
        {
            Debug.LogError("PlayerInput Trying to bind camera, but missing CameraOperation script on main camera");
        }
    }

    void SwitchParticleSystem(ParticleSystem particleSystem, bool state = true)
    {
        if (particleSystem)
            if (state)
                particleSystem.Play();
            else
                particleSystem.Stop();
    }

    [PunRPC]
    void SwitchVirusEffect(bool state)
    {
        SwitchParticleSystem(virusParticle, state);
    }

    [PunRPC]
    void SwitchCleanEffect()
    {
        SwitchParticleSystem(cleanParticle, true);
    }


    public void TriggerVirusEffect(bool state)
    {
        if (PhotonNetwork.OfflineMode)
            SwitchVirusEffect(state);
        else
            photonView.RPC("SwitchVirusEffect", RpcTarget.All, state);
    }

    public void TriggerCleanEffect()
    {
        if (PhotonNetwork.OfflineMode)
            SwitchCleanEffect();
        else
            photonView.RPC("SwitchCleanEffect", RpcTarget.All);
    }

}
