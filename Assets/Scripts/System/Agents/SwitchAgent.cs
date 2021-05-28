using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchAgent : MonoBehaviour
{
    public string switchName;
    public bool state;
    public UnityEvent onStateChanged;
    public UnityEvent onStateClose;
    public UnityEvent onStateOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch(bool nState)
    {
        if(this.state == nState)
            return;
        this.state = nState;
        if(this.state)
            onStateOpen.Invoke();
        else
            onStateClose.Invoke();
        onStateChanged.Invoke();
    }

    public void SwitchOnce()
    {
        Switch(!this.state);
    }
}
