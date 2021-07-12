using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public Vector3 positionOffset;
    public Vector3 spawnForce;
    public Transform spawnPointTransform;
    public bool spawnOnStart;

    void Start()
    {
        if (!spawnPointTransform)
            spawnPointTransform = this.transform;
        if(spawnOnStart)
            Spawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        GameObject obj;
        if (prefab)
        {
            obj = GameObject.Instantiate(prefab, spawnPointTransform.position + positionOffset, spawnPointTransform.rotation);
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            if(rigidbody)
                rigidbody.AddForce(spawnForce, ForceMode.Impulse);
        }
    }
}
