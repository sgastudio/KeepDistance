using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Main : MonoBehaviour
{
    public UnityEvent onPlay;
    public UnityEvent onOption;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void triggerPlay()
    {
        onPlay.Invoke();
    }
    public void triggerOption()
    {
        onOption.Invoke();
    }

    public void triggerExit()
    {
        Application.Quit();
    }
}
