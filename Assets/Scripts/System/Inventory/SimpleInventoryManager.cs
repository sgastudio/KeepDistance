using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class SimpleInventoryManager : Inventory
{
    [Header("Item")]
    public ItemAgent item;
    public Transform mountPoint;
    public Transform dropPoint;

    public override void AddItem(ItemAgent other)
    {
        if (item == null)
        {
            processItem(other);
        }
        else
        {
            DropItem(item, item.amount);
            processItem(other);
        }

        base.AddItem(other);
    }

    public override void DropItem(ItemAgent other, int amount)
    {
        //if (item == null)
        if (other == null)
            return;
        /*if (itemRigidbody && !kinematicState)
            itemRigidbody.isKinematic = false;
        */          

        ItemAgent targetItem;
        if(amount < other.amount)
        {
            targetItem = GameObject.Instantiate(other.gameObject).GetComponent<ItemAgent>();
            targetItem.SetInfo(amount);
            other.SetInfo(other.amount - amount);
        }
        else
        {
            targetItem = other;
        }

        if (dropPoint)
            targetItem.transform.SetPositionAndRotation(dropPoint.position, dropPoint.rotation);
        targetItem.transform.SetParent(null);
        //onItemDropped.Invoke(item.name);
        base.DropItem(targetItem, amount);
        item = null;
        //itemRigidbody = null;
    }
    public override void DropItemAll(ItemAgent other)
    {
        DropItem(item, item.amount);
    }

    public override int FindItem(string name)
    {
        if(item.itemName == name)
            return item.amount;
        else
            return -1;
    }

    public override ItemAgent GetItem(int index = 0)
    {
        return item;
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
        /*other.Attach(view.ViewID);*/
        /*if(mountPoint)
            other.Attach(mountPoint.transform);
        else
            other.Attach(this.transform);*/
        item = other;
    }
}
