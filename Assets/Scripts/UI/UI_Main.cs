using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class UI_Main : StackPanel
{
    //public UnityEvent onPlay;
    //public UnityEvent onOption;

    public void triggerPlay()
    {
        //onPlay.Invoke();
        TriggerNextPanel();
    }
    public void triggerOption()
    {
        //onOption.Invoke();
    }

    public void triggerExit()
    {
        Application.Quit();
    }
}
