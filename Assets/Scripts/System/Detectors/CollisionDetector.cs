using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Sirenix.OdinInspector;

public enum WorkMode
{
    Or,
    And
}
public class CollisionDetector : MonoBehaviour
{
    [FoldoutGroup("Filter")]
    //[Header("Filter")]
    public LayerMask layers;
    [FoldoutGroup("Filter")]
    public List<EnumTag> tags;
    [FoldoutGroup("Filter")]
    public WorkMode layerTagBlendMode;

    [FoldoutGroup("Networking")]
    public bool LocalPlayerOnly = false;
    [FoldoutGroup("Networking")]
    public PhotonView photonView;

    [FoldoutGroup("Events")]
    public UnityEvent<Collider> targetEnter;
    [FoldoutGroup("Events")]
    public UnityEvent<Collider> targetExit;
    [FoldoutGroup("Events")]
    public UnityEvent<Collider> targetStay;
    [FoldoutGroup("Events")]
    public UnityEvent<Collider> targetClean;


    [FoldoutGroup("Clean")]
    public float cleanInterval = 1f;
    [FoldoutGroup("Clean")]
    public float maxStayTimeLimit = 1f;

    [ReadOnly, Space]
    public List<GameObject> activeList;
    [ReadOnly]
    public Dictionary<GameObject, float> timeTable;

    IEnumerator ListWatcher()
    {
        yield return new WaitForSecondsRealtime(cleanInterval);
        UpdateList();
        StartCoroutine(ListWatcher());
    }

    void UpdateList()
    {
        float now = Time.unscaledTime;
        for(int i=0;i<activeList.Count;i++)
        {
            GameObject targetObj = activeList[i];
            if(timeTable[targetObj] + maxStayTimeLimit < now)
            {
                timeTable.Remove(targetObj);
                activeList.RemoveAt(i);
                targetClean.Invoke(targetObj.GetComponent<Collider>());
            }
        }
    }

    public virtual void Start()
    {
        timeTable = new Dictionary<GameObject, float>();
        StartCoroutine(ListWatcher());
    }

    public virtual void Awake()
    {
        activeList.Clear();

        if (this.GetComponentInParent<PhotonView>() && !photonView)
            photonView = this.GetComponentInParent<PhotonView>();
        else if (this.GetComponent<PhotonView>() && !photonView)
            photonView = this.GetComponent<PhotonView>();

        if (!photonView && LocalPlayerOnly)
            Debug.LogError(this.gameObject.ToString() + ": CollisionDetector missing component PhotonView during LocalPlayerOnly mode");
        else if (LocalPlayerOnly)
            Debug.Log(this.gameObject.ToString() + " Bind with component " + photonView + " in LocalPlayerOnly mode");
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (!GetNetworkingTest() && LocalPlayerOnly)
            return;
        if (GetBlendTest(other))
        {
            targetStay.Invoke(other);
            if(timeTable.ContainsKey(other.gameObject))
                timeTable[other.gameObject] = Time.unscaledTime;
            else
                OnTriggerEnter(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!GetNetworkingTest() && LocalPlayerOnly)
            return;
        if (GetBlendTest(other))
        {
            activeList.Remove(other.gameObject);
            timeTable.Remove(other.gameObject);
            targetExit.Invoke(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!GetNetworkingTest() && LocalPlayerOnly)
            return;
        if (GetBlendTest(other) && !activeList.Find(match=>{return match == other.gameObject;}))
        {
            activeList.Add(other.gameObject);
            if(timeTable.ContainsKey(other.gameObject))
                timeTable[other.gameObject] = Time.unscaledTime;
            else
                timeTable.Add(other.gameObject, Time.unscaledTime);
            targetEnter.Invoke(other);
        }
    }

    public bool GetBlendTest(Collider other)
    {
        switch (layerTagBlendMode)
        {
            case WorkMode.And:
                if (GetTagTest(other.gameObject) && GetLayerTest(other.gameObject.layer))
                    return true;
                break;
            case WorkMode.Or:
                if (GetTagTest(other.gameObject) || GetLayerTest(other.gameObject.layer))
                    return true;
                break;
            default:
                break;
        }
        return false;
    }
    public bool GetLayerTest(LayerMask l)
    {
        return (layers & l) != 0;
    }

    public bool GetTagTest(GameObject obj)
    {
        bool Result = false;

        foreach (EnumTag tag in tags)
        {
            Result |= obj.CompareTag(tag.ToString());
        }
        return Result;
    }

    public bool GetNetworkingTest()
    {
        //Debug.Log(photonView);
        //if (LocalPlayerOnly)
        if (photonView)
            return photonView.IsMine;
        else if (LocalPlayerOnly)
        {
            Debug.LogError(this.gameObject.ToString() + ": CollisionDetector missing component PhotonView during LocalPlayer Only mode");
            return true;
        }
        else
        {
            return true;
        }
        //else
        //    return true;
    }
}
