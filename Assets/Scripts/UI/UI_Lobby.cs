using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UI_Lobby : StackPanel
{
    public Dropdown roomSelector;
    public void triggerBack()
    {
        networkManager.Disconnect();
    }

    public void triggerJoin()
    {
        //onJoin.Invoke();
        TriggerNextPanel("Panel_Join");
    }

    public void triggerRandomJoin()
    {
        //onRefresh.Invoke();
    }

    public void triggerCreate()
    {
        TriggerNextPanel("Panel_Create");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        TriggerLastPanel();
    }

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        Debug.Log("PUN called OnRoomListUpdate()");
        if(!roomSelector)
            return;
        roomSelector.ClearOptions();
        roomSelector.options = rooms.ConvertAll<Dropdown.OptionData>(result=>{
            return new Dropdown.OptionData(result.Name +" - "+ result.PlayerCount.ToString() +"/"+ result.MaxPlayers.ToString());
        });
    }
}
