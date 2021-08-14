using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAgent : MonoBehaviour
{
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag(EnumTag.GameController.ToString()).Length>1)
            GameObject.Destroy(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
