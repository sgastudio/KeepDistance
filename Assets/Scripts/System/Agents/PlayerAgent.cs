using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public string playerName;
    public int playerGroup;

    // Start is called before the first frame update
    void Start()
    {
        if(string.IsNullOrEmpty(playerName))
            playerName = this.gameObject.name + this.gameObject.GetHashCode().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
