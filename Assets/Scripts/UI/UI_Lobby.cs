using System;
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

    private void Update()
    {
        /*if (networkManager && roomSelector)
            if(networkManager.roomList.Count != roomSelector.options.Count)
                UpdateRoomDropdown();*/
    }

    public void triggerBack()
    {
        networkManager.Disconnect();
    }

    public void triggerJoin()
    {
        //onJoin.Invoke();
        if (networkManager && roomSelector)
            if (roomSelector.options.Count > 0)
                networkManager.JoinRoom(roomSelector.options[roomSelector.value].text);
        //TODO: else if password required

    }

    public void triggerRandomJoin()
    {
        //onRefresh.Invoke();
        networkManager.JoinRoomRandom();
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
        //UpdateRoomDropdown(networkManager.roomList);
        UpdateRoomDropdown(rooms);
    }

    public void UpdateRoomDropdown(List<RoomInfo> rooms)
    {
        if (!roomSelector || !networkManager)
            return;
        roomSelector.ClearOptions();
        roomSelector.options = networkManager.roomList.ConvertAll<Dropdown.OptionData>(result =>
        {
            //return new Dropdown.OptionData(result.Name + " - " + result.PlayerCount.ToString() + "/" + result.MaxPlayers.ToString());
            return new Dropdown.OptionData(result.Name);
        });
    }

    public override void OnJoinedRoom()
    {
        TriggerNextPanel("Panel_Room");
    }
}
