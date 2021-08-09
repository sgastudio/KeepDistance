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
    SceneControl sceneControl;

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 20;

    // Start is called before the first frame update
    void Start()
    {
        if (!sceneControl)
            sceneControl = GetComponent<SceneControl>();
        if (!sceneControl)
            Debug.LogError("NetworkManager missing component SceneControl");
        ClientState state = PhotonNetwork.NetworkClientState;
        //make compatibility for self-hold sever
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.SerializationProtocolType = ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TestCreate()
    {
        Connect();
        CreateRoom(null);
        //StartArena();
    }
    public void TestJoin()
    {
        Connect();
        PhotonNetwork.JoinRandomRoom();
    }
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        if (string.IsNullOrEmpty(clientNickName))
            clientNickName = "Guest" + GetInstanceID().ToString();
        PhotonNetwork.NickName = clientNickName;
    }

    public void CreateRoom(string roomName)
    {
        Debug.Log("PUN Creating room with name - " + roomName);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        //PhotonNetwork.JoinLobby();
        //PhotonNetwork.GetCustomRoomList(TypedLobby.Default,"Select *");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(string room, string[] expectedUsers = null)
    {
        PhotonNetwork.JoinRoom(room, expectedUsers);
    }

    public void StartArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PUN Try to load level but not the master client");
            return;
        }


        PhotonNetwork.LoadLevel(EnumLevel.Loading.ToString());

        //Level selection here
        if (sceneControl)
            sceneControl.nextNetworkScene = EnumLevel.Level_Tutorial.ToString();
    }

    public void StartTutorial()
    {
        PhotonNetwork.OfflineMode = true;

        PhotonNetwork.LoadLevel(EnumLevel.Loading.ToString());

        //Level selection here
        if (sceneControl)
            sceneControl.nextNetworkScene = EnumLevel.Level_Tutorial.ToString();
    }

    public void StartMainMenu()
    {
        PhotonNetwork.OfflineMode=true;

        PhotonNetwork.LoadLevel(EnumLevel.Loading.ToString());

        //Level selection here
        if (sceneControl)
            sceneControl.nextScene = EnumLevel.MainMenu.ToString();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void GetLobby()
    {
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "Select *");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Connected and calling OnConnectedTOMaster()");
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        base.OnDisconnected(cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN called OnJoinRandomFailed(), but no room avaliable");
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN called OnJoinedRoom(), Room joined");
        base.OnJoinedRoom();
    }

    public void SetNickName(string name)
    {
        clientNickName = name;
    }

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        roomList = rooms;
        base.OnRoomListUpdate(rooms);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
