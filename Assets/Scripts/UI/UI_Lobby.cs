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

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        roomSelector.ClearOptions();
        roomSelector.options = rooms.ConvertAll<Dropdown.OptionData>(result=>{
            return new Dropdown.OptionData(result.Name + result.PlayerCount.ToString() +"/"+ result.MaxPlayers.ToString());
        });
    }
}
