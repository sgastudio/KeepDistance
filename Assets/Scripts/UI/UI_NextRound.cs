using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NextRound : StackPanel
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Cursor.visible  = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerNextRound()
    {
        if (networkManager)
        {
            networkManager.LeaveRoom();
            networkManager.StartMainMenu(3);
        }
    }

    public void TriggerMainMenu()
    {
        if (networkManager)
        {
            networkManager.Disconnect();
            networkManager.StartMainMenu(1);
        }
    }

    public void TriggerExit()
    {
        Application.Quit();
    }
}
