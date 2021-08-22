using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;

public class SceneControl : MonoBehaviour
{
    [Scene, ROA]
    public string nextScene;
    [Scene, ROA]
    public string nextNetworkScene;
    [Scene]
    public string[] levels;
    [Scene]
    public string resultScene;
    [Scene]
    public string menuScene;
    [Scene]
    public string loadScene;

    public static GameObject LocalPlayerInstance;
    public static SceneControl Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        GameObject controller = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        // if (!dataManager && controller)
        //     dataManager = controller.GetComponent<DataManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerFailed()
    {
        Debug.Log("Failed!");
        DataManager.SetData("level result", 0, "fail", true);
        if (!string.IsNullOrWhiteSpace(resultScene))
        {
            nextScene = resultScene;
            SceneManager.LoadScene(loadScene);
        }

    }
    public void TriggerWin()
    {
        Debug.Log("Win!");
        DataManager.SetData("level result", 1, "win", true);
        if (!string.IsNullOrWhiteSpace(resultScene))
        {
            nextScene = resultScene;
            SceneManager.LoadScene(EnumLevel.Loading.ToString());
        }

    }

    public void TriggerNextLevel()
    {
        Debug.Log("Next Level!");
        if (!string.IsNullOrWhiteSpace(nextScene))
            SceneManager.LoadScene(loadScene);
    }


    public void LoadLocal(EnumLevel level)
    {
        nextScene = level.ToString();
        SceneManager.LoadScene(EnumLevel.Loading.ToString());
    }

    public void LoadNetwork(EnumLevel level)
    {
        nextNetworkScene = level.ToString();
        PhotonNetwork.LoadLevel(EnumLevel.Loading.ToString());

    }
}
