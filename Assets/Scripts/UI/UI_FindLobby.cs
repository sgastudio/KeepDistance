using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UI_FindLobby : MonoBehaviour
{
    public UnityEvent onBack;
    public UnityEvent onJoin;
    public UnityEvent onCreate;
    public UnityEvent onRefresh;
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

    public void triggerJoin()
    {
        onJoin.Invoke();
    }

    public void triggerRefresh()
    {
        onRefresh.Invoke();
    }

    public void triggerCreate()
    {
        onCreate.Invoke();
    }

}
