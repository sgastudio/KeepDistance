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
public class LineDetector : MonoBehaviour
{
    public GameObject linePrefab;
    public string targetTag = "Player";
    public LayerMask targetLayer = 0;
    [ROA]
    public List<LinePair> playerList;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag || (other.gameObject.layer & targetLayer) != 0)
        {
            Debug.Log("Player Entered");
            playerList.Add(new LinePair(other.gameObject, linePrefab, this.transform));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag || (other.gameObject.layer & targetLayer) != 0)
        {
            Debug.Log("Player Leaved");
            LinePair pair = playerList.Find(result=>{
                return (result.Object == other.gameObject);
            });
            
            playerList.Remove(pair);
            GameObject.Destroy(pair.LineObject);
        }
    }
}
