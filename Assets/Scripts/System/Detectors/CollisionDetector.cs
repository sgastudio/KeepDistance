using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public enum WorkMode
{
    Or,
    And
}
public class CollisionDetector : MonoBehaviour
{
    [Header("Filter")]
    public LayerMask layers;
    public List<EnumTag> tags;
    public WorkMode layerTagBlendMode;
    public bool LocalPlayerOnly = false;

    [Header("Event")]
    public UnityEvent<Collider> targetEnter;
    public UnityEvent<Collider> targetExit;

    [ROA, Space]
    public List<GameObject> activeList;

    public PhotonView photonView;


    public virtual void Start()
    {
        
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

    void OnTriggerExit(Collider other)
    {
        if (!GetNetworkingTest() && LocalPlayerOnly)
            return;
        if (GetBlendTest(other))
        {
            activeList.Remove(other.gameObject);
            targetExit.Invoke(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!GetNetworkingTest() && LocalPlayerOnly)
            return;
        if (GetBlendTest(other))
        {
            activeList.Add(other.gameObject);
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
