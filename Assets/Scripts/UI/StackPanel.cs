using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class StackPanel : MonoBehaviourPunCallbacks
{
    public NetworkManager networkManager;
    public GameObject lastPanel;
    public GameObject nextPanel;

    public UnityEvent<GameObject> onNextPanel;
    public UnityEvent<GameObject> onLastPanel;

    // Start is called before the first frame update
    void Start()
    {
        if(!networkManager)
            networkManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<NetworkManager>();
        if(lastPanel)
            this.gameObject.SetActive(false);
    }

    public void TriggerNextPanel()
    {
        if(!nextPanel)
            return;
        this.nextPanel.SetActive(true);
        this.gameObject.SetActive(false);
        onNextPanel.Invoke(nextPanel);
    }

     public void TriggerLastPanel()
    {
        if(!lastPanel)
            return;
        this.lastPanel.SetActive(true);
        this.gameObject.SetActive(false);
        onLastPanel.Invoke(lastPanel);
    }
}
