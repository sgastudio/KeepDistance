using System.Runtime.CompilerServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public string gameVersion = "0.1";
    public string clientNickName;
    public List<RoomInfo> roomList;

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 20;

    // Start is called before the first frame update
    void Start()
    {
        ClientState state = PhotonNetwork.NetworkClientState;
        //make compatibility for self-hold sever
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.SerializationProtocolType = ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName  = clientNickName;
    }

    public void CreateRoom() 
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
        //PhotonNetwork.JoinLobby();
        //PhotonNetwork.GetCustomRoomList(TypedLobby.Default,"Select *");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(string room,string[] expectedUsers = null)
    {
        PhotonNetwork.JoinRoom(room, expectedUsers);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void GetLobby()
    {
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default,"Select *");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Connected and calling OnConnectedTOMaster()");
        PhotonNetwork.JoinLobby();
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN called OnJoinRandomFailed(), but no room avaliable");
       
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN called OnJoinedRoom(), Room joined");
    }

    public void SetNickName(string name)
    {
        clientNickName = name;
    }

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        roomList = rooms;
    }
}
