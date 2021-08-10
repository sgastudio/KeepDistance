using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public Vector3 positionOffset;
    public Vector3 spawnForce;
    public float forceRandomize;
    Vector3 forceOffset;
    public Transform spawnPointTransform;
    public bool spawnOnStart;

    public float loopInterval = 0.0f;
    public float loopRandomize = 0.0f;
    float intervalOffset;
    public int loopTimes = 0;
    int spawnedTimes;
    float lastSpawnTime;

    void Start()
    {
        if (!spawnPointTransform)
            spawnPointTransform = this.transform;
        if (spawnOnStart)
            Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (loopInterval > 0 && ((loopTimes > 0 && spawnedTimes < loopTimes) || loopTimes == 0))
        {
            if (lastSpawnTime + loopInterval + intervalOffset <= Time.time)
                Spawn();
        }
    }

    public void Spawn()
    {
        GameObject obj;
        if (prefab)
        {
            obj = GameObject.Instantiate(prefab, spawnPointTransform.position + positionOffset, spawnPointTransform.rotation);
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            
            if (rigidbody)
            {
                forceOffset = Random.insideUnitSphere * forceRandomize;
                rigidbody.AddForce(spawnForce + forceOffset, ForceMode.Impulse);
            }
        }
        lastSpawnTime = Time.time;
        spawnedTimes += 1;
        intervalOffset = Random.Range(-loopRandomize, loopRandomize);
    }
}
