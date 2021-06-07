using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneLoader : MonoBehaviour
{
    SceneControl sceneControl;
    DataManager dataManager;
    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject SysObject = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        sceneControl = SysObject.GetComponent<SceneControl>();
        dataManager = SysObject.GetComponent<DataManager>();
        networkManager = SysObject.GetComponent<NetworkManager>();
        if (sceneControl && !string.IsNullOrWhiteSpace(sceneControl.nextNetworkScene))
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PUN Try to load level but not the master client");
                return;
            }
            PhotonNetwork.LoadLevel(sceneControl.nextNetworkScene);
            sceneControl.nextScene = null;
        }
        else if (sceneControl && !string.IsNullOrWhiteSpace(sceneControl.nextScene))
        {
            SceneManager.LoadSceneAsync(sceneControl.nextScene);
            sceneControl.nextScene = null;
        }
    }
}
