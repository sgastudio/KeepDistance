using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;

public class UI_FindLobby : StackPanel
{
    public void triggerBack()
    {
        networkManager.Disconnect();
    }

    public void triggerJoin()
    {
        //onJoin.Invoke();
    }

    public void triggerRefresh()
    {
        //onRefresh.Invoke();
    }

    public void triggerCreate()
    {
        networkManager.CreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        TriggerLastPanel();
    }
}
