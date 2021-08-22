using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class LinePair
{
    public LinePair(GameObject target, GameObject linePrefab, Transform parentTrans)
    {
        Object = target;
        if (linePrefab)
        {
            //LineObject = GameObject.Instantiate(linePrefab, parent.transform.position, parent.transform.rotation, parent.transform);
            LineObject = GameObject.Instantiate(linePrefab, parentTrans, false);
            LineObject.GetComponent<Line>().target = target;
        }

    }
    ~LinePair()
    {
        if (LineObject)
            GameObject.Destroy(LineObject);
    }
    public GameObject Object;
    public GameObject LineObject;
}

[RequireComponent(typeof(SphereCollider))]
public class LineDetector : CollisionDetector
{
    [Header("Components")]
    public GameObject linePrefab;
    [ROA]
    public List<LinePair> playerList;
    public float distance
    {
        get
        {
            return this.GetComponent<SphereCollider>().radius;
        }
    }

    public override void Start()
    {
        base.targetEnter.AddListener(TriggerEnter);
        base.targetExit.AddListener(TriggerExit);
        base.Start();
    }

    public void TriggerEnter(Collider other)
    {
        if (!GetNetworkingTest())
            return;
        playerList.Add(new LinePair(other.gameObject, linePrefab, this.transform));
    }
    public void TriggerExit(Collider other)
    {
        if (!GetNetworkingTest())
            return;
        LinePair pair = playerList.Find(result =>
        {
            return (result.Object == other.gameObject);
        });
        if(pair == null)
            return;
        playerList.Remove(pair);
        GameObject.Destroy(pair.LineObject);
    }
}
