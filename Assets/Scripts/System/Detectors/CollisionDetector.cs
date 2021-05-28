using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    public enum FilterMode
    {
        Or,
        And
    }
    [Header("Filter")]
    public LayerMask layers;
    public List<EnumTag> tags;
    public FilterMode layerTagBlendMode;

    [Header("Event")]
    public UnityEvent targetEnter;
    public UnityEvent targetExit;

    [ROA, Space]
    public List<GameObject> activeList;

    // Start is called before the first frame update
    void Start()
    {
        activeList.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit(Collider other)
    {
        if (blendTest(other))
        {
            activeList.Remove(other.gameObject);
            targetExit.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (blendTest(other))
        {
            activeList.Add(other.gameObject);
            targetEnter.Invoke();
        }
    }

    bool blendTest(Collider other)
    {
        switch (layerTagBlendMode)
        {
            case FilterMode.And:
                if (GetTagTest(other.gameObject) && GetLayerTest(other.gameObject.layer))
                    return true;
                break;
            case FilterMode.Or:
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
