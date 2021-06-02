using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    SceneControl sceneControl;
    DataManager dataManager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject SysObject = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        sceneControl = SysObject.GetComponent<SceneControl>();
        dataManager =SysObject.GetComponent<DataManager>();
        if(sceneControl && !string.IsNullOrWhiteSpace(sceneControl.nextScene))
        {
            SceneManager.LoadSceneAsync(sceneControl.nextScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
