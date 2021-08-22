using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour
{
    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject systemObject = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if(!systemObject)
        {
            Debug.LogWarning("System Control Object not detected!");
            return;
        }
        networkManager = systemObject.GetComponent<NetworkManager>();
        if(!networkManager)
        {
            Debug.LogError("Missing Network manager on System Control Object");
        }
    }


    public void TriggerEnd()
    {
        if(networkManager)
            networkManager.StartMainMenu(1);
    }
}
