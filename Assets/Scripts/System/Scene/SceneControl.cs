using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    DataManager dataManager;
    [Scene,ROA]
    public string nextScene;
    [Scene,ROA]
    public string nextNetworkScene;
    [Scene]
    public string[] levels;
    [Scene]
    public string resultScene;
    [Scene]
    public string menuScene;
    [Scene]
    public string loadScene;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.FindWithTag(EnumTag.GameController.ToString()).GetComponent<DataManager>();
        //SceneManager.LoadSceneAsync(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerFailed()
    {
        Debug.Log("Failed!");
        if (dataManager)
            dataManager.SetData("level result", 0, "fail", true);
        if (!string.IsNullOrWhiteSpace(resultScene))
        {
            nextScene = resultScene;
            SceneManager.LoadScene(loadScene);
        }

    }
    public void TriggerWin()
    {
        Debug.Log("Win!");
        if (dataManager)
            dataManager.SetData("level result", 1, "win", true);
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
}
