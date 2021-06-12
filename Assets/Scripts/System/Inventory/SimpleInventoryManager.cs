using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SimpleInventoryManager : MonoBehaviour
{
    public ItemAgent item;
    public Transform mountPoint;
    bool kinematicState = false;
    Rigidbody itemRigidbody = null;

    [Header("Events")]
    public UnityEvent<string> onItemAdded;
    public UnityEvent<string> onItemDropped;

    public void AddItem(ItemAgent other)
    {
        if (item == null)
        {
            processItem(other);
        }
        else
        {
            DropItem();
            processItem(other);
        }

        onItemAdded.Invoke(other.name);
    }

    public void DropItem()
    {
        if (item == null)
            return;
        if (itemRigidbody && !kinematicState)
            itemRigidbody.isKinematic = false;
        item.transform.SetParent(null);
        onItemDropped.Invoke(item.name);
        item = null;
    }

    void processItem(ItemAgent other)
    {
        itemRigidbody = other.GetComponent<Rigidbody>();
        if (itemRigidbody)
        {
            kinematicState = itemRigidbody.isKinematic;
            itemRigidbody.isKinematic = true;
        }
        if (mountPoint)
        {
            other.transform.SetPositionAndRotation(mountPoint.position, mountPoint.rotation);
            other.transform.SetParent(mountPoint.transform);
        }
        else
        {
            other.transform.SetParent(this.transform);
        }
    }

}
