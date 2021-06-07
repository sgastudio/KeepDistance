using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Create : StackPanel
{
    public InputField roomNameField;
    public InputField roomPasswordField;
    public void triggerBack()
    {
        TriggerLastPanel();
    }

    public void triggerCreate()
    {
        if (networkManager)
            if(roomNameField && !string.IsNullOrWhiteSpace(roomNameField.text))
                networkManager.CreateRoom(roomNameField.text);
            else
                networkManager.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        TriggerNextPanel();
    }
}
