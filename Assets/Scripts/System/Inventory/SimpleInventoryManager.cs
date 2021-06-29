using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SimpleInventoryManager : Inventory
{
    [Header("Item")]
    public ItemAgent item;
    public Transform mountPoint;
    public Transform dropPoint;
    //bool kinematicState = false;
    //Rigidbody itemRigidbody = null;

    public override void AddItem(ItemAgent other)
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

        base.AddItem(other);
    }

    public override void DropItem(ItemAgent other=null)
    {
        if (item == null)
            return;
        /*if (itemRigidbody && !kinematicState)
            itemRigidbody.isKinematic = false;
*/
        if (dropPoint)
            item.gameObject.transform.SetPositionAndRotation(dropPoint.position, dropPoint.rotation);
        item.transform.SetParent(null);
        onItemDropped.Invoke(item.name);
        base.DropItem(item);
        item = null;
        //itemRigidbody = null;
    }

    public override int FindItem(string name)
    {
        if(item.name == name)
            return 0;
        else
            return -1;
    }

    void processItem(ItemAgent other)
    {
        //itemRigidbody = other.GetComponent<Rigidbody>();
        /*if (itemRigidbody)
        {
            kinematicState = itemRigidbody.isKinematic;
            itemRigidbody.isKinematic = true;
        }*/
        if (mountPoint)
        {
            other.transform.SetPositionAndRotation(mountPoint.position, mountPoint.rotation);
            other.transform.SetParent(mountPoint.transform);
        }
        else
        {
            other.transform.SetParent(this.transform);
        }
        item = other;
    }

}
