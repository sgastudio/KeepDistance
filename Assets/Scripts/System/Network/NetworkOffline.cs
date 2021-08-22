using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkOffline : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()))
        PhotonNetwork.OfflineMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        PhotonNetwork.OfflineMode = false;
    }
}
