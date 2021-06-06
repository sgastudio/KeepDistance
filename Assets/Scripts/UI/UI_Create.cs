using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Create : StackPanel
{
    public void triggerBack()
    {
        TriggerLastPanel();
    }

    public void triggerCreate()
    {
        if (networkManager)
            networkManager.CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        TriggerNextPanel();
    }
}
