using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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

    void TriggerFailed()
    {
        Debug.Log("Failed!");
    }
    void TriggerWin()
    {
        Debug.Log("Win!");
    }
}
