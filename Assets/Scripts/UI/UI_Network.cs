using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;

public class UI_Network : StackPanel
{
    public InputField userNameField;
    public void triggerBack()
    {
        TriggerLastPanel();
    }

    public void triggerConnect()
    {
        if(userNameField)
            networkManager.SetNickName(string.IsNullOrEmpty(userNameField.text)?"Guest"+GetInstanceID():userNameField.text);
        if(networkManager)
            networkManager.Connect();

    }

    public override void OnConnectedToMaster()
    {
        TriggerNextPanel();
    }
    
}
