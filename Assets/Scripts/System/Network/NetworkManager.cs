using System.Data;
using System.Runtime.CompilerServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "0.1";
    public string clientNickName;
    public int gender;
    public GameObject localPlayerObject;
    public List<RoomInfo> roomList;
    SceneControl sceneControl;

    public const string MAP_PROP_KEY = "map";
    public const string GAME_MODE_PROP_KEY = "gm";
    public const string GAME_STARTED_KEY = "st";

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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { MAP_PROP_KEY, GAME_MODE_PROP_KEY, GAME_STARTED_KEY };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { MAP_PROP_KEY, (byte)1 }, { GAME_MODE_PROP_KEY, (byte)0 }, { GAME_STARTED_KEY, (byte)0 } };
        roomOptions.MaxPlayers = maxPlayersPerRoom;
        //PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        PhotonNetwork.CreateRoom(roomName, roomOptions);
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

    public void JoinRoomRandom()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable { { GAME_STARTED_KEY, (byte)0 } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayersPerRoom);
    }

    public void StartArena()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("PUN Try to load level but not the master client");
            return;
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { GAME_STARTED_KEY, (byte)1 } });

        //Level selection here
        if (sceneControl)
            sceneControl.LoadNetwork(EnumLevel.Level_city);
    }

    public void StartTutorial()
    {
        if (sceneControl)
            sceneControl.LoadLocal(EnumLevel.Level_Tutorial);
    }

    public void StartMainMenu(int menuEntry)
    {
        DataManager.SetData("entry", menuEntry, "", true);

        if (sceneControl)
            sceneControl.LoadLocal(EnumLevel.MainMenu);
    }

    public void StartResult()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("PUN Try to load level but not the master client");
            return;
        }

        //Level selection here
        if (sceneControl)
            sceneControl.LoadNetwork(EnumLevel.Result);
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
        base.OnConnectedToMaster();
        if(PhotonNetwork.OfflineMode)
            return;
        Debug.Log("PUN Connected and calling OnConnectedTOMaster()");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        JoinRoomRandom();
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
        CreateRoom(clientNickName + Time.time.ToString());
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

    public void SetGender(int g)
    {
        gender = g;
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log("Player "+targetPlayer.NickName+" updated "+ changedProps.ToStringFull());
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        Debug.Log("Room updated " + propertiesThatChanged.ToStringFull());
    }
}
