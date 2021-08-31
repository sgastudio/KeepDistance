using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UI_Room : StackPanel
{
    public UnityEngine.UI.Button startButton;

    public override void Start()
    {
        base.Start();
        if (startButton)
            if (!PhotonNetwork.IsMasterClient)
                startButton.enabled = false;
            else
                startButton.enabled = true;
    }

    #region public triggers
    public void triggerBack()
    {
        if (networkManager)
        {
            networkManager.LeaveRoom();
            networkManager.Disconnect();
        }
    }

    public void triggerStart()
    {
        if (networkManager && PhotonNetwork.IsMasterClient)
            networkManager.StartArena();
    }
    #endregion

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        TriggerLastPanel();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        //TriggerLastPanel();
    }
}
