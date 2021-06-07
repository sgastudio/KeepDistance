using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UI_Join : StackPanel
{
    public Dropdown roomSelector;
    public string selectedRoom;
    public void triggerBack()
    {
        TriggerLastPanel();
    }

    public void triggerJoin()
    {
        if(networkManager)
            networkManager.JoinRoom(selectedRoom);
    }

    public override void OnJoinedRoom()
    {
        TriggerNextPanel();
    }
}
