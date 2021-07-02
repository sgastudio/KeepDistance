using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Inventory : MonoBehaviour
{
    /*
    public Transform mountPoint;
    public Transform dropPoint;
    bool kinematicState = false;
    Rigidbody itemRigidbody = null;*/

    [Header("Events")]
    public UnityEvent<string> onItemAdded;
    public UnityEvent<string> onItemDropped;

    public virtual void AddItem(ItemAgent other)
    {
        SetPhysicsState(other, false);
        onItemDropped.Invoke(other.name);
    }

    public virtual void DropItem(ItemAgent other, int amount)
    {
        SetPhysicsState(other, true);
        onItemDropped.Invoke(other.name);
    }

    public virtual void DropItemAll(ItemAgent other)
    {
        SetPhysicsState(other, true);
        onItemDropped.Invoke(other.name);
    }

    public virtual int FindItem(string name)
    {
        return -1;
    }

    public virtual ItemAgent GetItem(int index = 0)
    {
        return null;
    }

    public virtual Transform GetMountTransform(int index) 
    {
        return null;
    }
    public virtual Transform GetDropTransform() 
    {
        return this.transform;
    }

    void SetPhysicsState(ItemAgent other, bool state)
    {
        Collider collider = other.GetComponent<Collider>();
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();

        if (collider)
            collider.enabled = state;
        if (rigidbody)
            rigidbody.isKinematic = !state;
    }

}
