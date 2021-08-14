using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelPlayerSpawner : MonoBehaviour
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;
    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if (!networkManager && controller)
            networkManager = controller.GetComponent<NetworkManager>();
        else
            return;

        if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
            {
                GameObject playerObject = PhotonNetwork.Instantiate(networkManager.gender > 0 ? boyPrefab.name : girlPrefab.name, Vector3.zero, Quaternion.identity, 0);
                networkManager.localPlayerView = playerObject.GetComponent<PhotonView>().ViewID;
                PlayerAgent agent = playerObject.GetComponent<PlayerAgent>();
                agent.playerName = networkManager.clientNickName;
                agent.gender = networkManager.gender;
            }

    }
}
