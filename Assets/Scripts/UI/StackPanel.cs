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
    public StackPanel lastPanel;
    public StackPanel[] nextPanel;

    public UnityEvent<StackPanel> onNextPanel;
    public UnityEvent<StackPanel> onLastPanel;

    // Start is called before the first frame update
    public virtual void Start()
    {
        // if (!networkManager)
        //     networkManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<NetworkManager>();
    }

    public virtual void Awake()
    {
        //if (lastPanel)
        //    this.gameObject.SetActive(false);
        if (!networkManager)
            networkManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<NetworkManager>();
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
        this.nextPanel[index].gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        onNextPanel.Invoke(nextPanel[index]);
    }

    public void TriggerLastPanel()
    {
        if (!lastPanel)
            return;
        this.lastPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        onLastPanel.Invoke(lastPanel);
    }
}
