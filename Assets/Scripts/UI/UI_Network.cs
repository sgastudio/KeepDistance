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
            if(string.IsNullOrEmpty(userNameField.text))
            {
                string tempName = "Guest"+GetInstanceID();
                networkManager.SetNickName(tempName);
                userNameField.text = tempName; 
            }
            else
            {
                networkManager.SetNickName(userNameField.text);
            }
        if(networkManager)
            networkManager.Connect();

    }

    public override void OnConnectedToMaster()
    {
        TriggerNextPanel();
    }
    

    public void TriggerChangeNickName(string name)
    {
        if(networkManager)
            networkManager.SetNickName(userNameField.text);
    }
}
