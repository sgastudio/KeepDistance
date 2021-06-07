using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Room : StackPanel
{
    #region public triggers
        public void triggerBack()
        {
            if(networkManager)
                networkManager.LeaveRoom();
        }

        public void triggerStart()
        {
            if(networkManager)
                networkManager.StartArena();
        }
    #endregion

    public override void OnLeftRoom()
    {
        TriggerLastPanel();
    }
}
