using System;
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
    public GameObject[] nextPanel;

    public UnityEvent<GameObject> onNextPanel;
    public UnityEvent<GameObject> onLastPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!networkManager)
            networkManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<NetworkManager>();
    }

    void Awake()
    {
        if (lastPanel)
            this.gameObject.SetActive(false);
    }

    public void TriggerNextPanel(string panelObjectName)
    {
        int index = Array.FindIndex(nextPanel,result=>{
            return result.name == panelObjectName;
        });
        if(index>=0)
        TriggerNextPanel(index);
    }
    public void TriggerNextPanel(int index = 0)
    {
        if (!nextPanel[index])
            return;
        this.nextPanel[index].SetActive(true);
        this.gameObject.SetActive(false);
        onNextPanel.Invoke(nextPanel[index]);
    }

    public void TriggerLastPanel()
    {
        if (!lastPanel)
            return;
        this.lastPanel.SetActive(true);
        this.gameObject.SetActive(false);
        onLastPanel.Invoke(lastPanel);
    }
}
