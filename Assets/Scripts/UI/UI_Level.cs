using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class UI_Level : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject playerPrefab;

    void Start()
    {
       GameObject controller = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if (!networkManager && controller)
            networkManager = controller.GetComponent<NetworkManager>();
    }
    public void TriggerTest1()
    {
        networkManager.TestCreate();
    }
    public void TriggerTest2()
    {
        networkManager.TestJoin();
    }

    public void TriggerTest3()
    {
        PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,Quaternion.identity,0);
    }
}
