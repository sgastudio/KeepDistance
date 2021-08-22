using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    public float duration;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject,duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
