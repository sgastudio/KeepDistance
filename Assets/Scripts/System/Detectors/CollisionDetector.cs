using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Event")]
    public UnityEvent<Collider> targetEnter;
    public UnityEvent<Collider> targetExit;

    [ROA, Space]
    public List<GameObject> activeList;

    // Start is called before the first frame update
    void Start()
    {
        activeList.Clear();
    }

    void OnTriggerExit(Collider other)
    {
        if (blendTest(other))
        {
            activeList.Remove(other.gameObject);
            targetExit.Invoke(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (blendTest(other))
        {
            activeList.Add(other.gameObject);
            targetEnter.Invoke(other);
        }
    }

    bool blendTest(Collider other)
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
}
