using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;


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
        if (PhotonNetwork.IsMasterClient)
            if (DataManager.ContainData("result"))
                systemObjet.GetComponent<NetworkManager>().StartResult();
    }

    [PunRPC]
    void SetResult(int viewID)
    {
        Debug.Log("Result View ID:" + viewID.ToString());
        if (viewID != -1) 
        {
            PhotonView view = PhotonView.Find(viewID);
            PlayerAgent agent = view.GetComponent<PlayerAgent>(); ;
            DataManager.SetData("result", agent.gender, agent.playerName, true);
        }
        else
        {
            DataManager.SetData("result", -1, "", true);
        }
    }

    public void SyncResult(int viewID)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {
            SetResult(viewID);
        }
        else
        {
            photonView.RPC("SetResult", RpcTarget.All, viewID);
        }
    }

    public void TriggerWin()
    {
        SyncResult(networkManager.localPlayerView);
    }

    public void TriggerFailed()
    {
        SyncResult(-1);
    }
}
