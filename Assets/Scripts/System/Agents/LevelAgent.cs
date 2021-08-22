using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;


public class LevelAgent : MonoBehaviourPun
{
    GameObject systemObjet;
    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        systemObjet = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if (systemObjet)
            networkManager = systemObjet.GetComponent<NetworkManager>();
    }

    void Update()
    {

    }

    [PunRPC]
    void SetResult(int gender , string name)
    {
        Debug.Log("Result player "+name+"g:" + gender.ToString());
        //Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerID, true);
        if (gender>=0)
        {
            DataManager.SetData("result", gender, name, true);
        }
        else
        {
            DataManager.SetData("result", -1, "", true);
        }
        if(networkManager)
            networkManager.StartResult();
    }

    public void SyncResult(int gender, string name)
    {
        //int playerID = player != null ? player.ActorNumber : -1;
        if (!PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {
            SetResult(gender,name);
        }
        else
        {
            photonView.RPC("SetResult", RpcTarget.All, gender, name);
        }
    }

    public void TriggerWin()
    {
        SyncResult((int)(byte)PhotonNetwork.LocalPlayer.CustomProperties[PlayerAgent.PLAYER_GENDER_KEY],PhotonNetwork.LocalPlayer.NickName);
    }

    public void TriggerFailed()
    {
        SyncResult(-1, null);
    }
}
