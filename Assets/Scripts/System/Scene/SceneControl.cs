using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControl : MonoBehaviour
{
    public DataManager dataManager;
    public string nextScene;
    public string winScene;
    public string failScene;
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

    void OnDestroy()
    {
        return;
    }

    public void TriggerFailed()
    {
        Debug.Log("Failed!");
        if(dataManager)
            dataManager.SetData("level result",0,"fail",true);
        if(!string.IsNullOrWhiteSpace(failScene))
            SceneManager.LoadScene(failScene);
    }
    public void TriggerWin()
    {
        Debug.Log("Win!");
        if(dataManager)
            dataManager.SetData("level result",1,"win",true);
         if(!string.IsNullOrWhiteSpace(winScene))
            SceneManager.LoadScene(winScene);
    }
}
