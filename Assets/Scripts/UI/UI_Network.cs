using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Network : MonoBehaviour
{
    public UnityEvent onBack;
    public UnityEvent onConnect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void triggerBack()
    {
        onBack.Invoke();
    }
    public void triggerConnect()
    {
        onConnect.Invoke();
    }
}
