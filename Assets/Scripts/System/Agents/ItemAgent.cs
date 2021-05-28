using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAgent : MonoBehaviour
{
    public string itemName;
    [Min(1)]
    public int amount = 1;
    public ItemType type;

    public void SetInfo(string name,int amount,ItemType type)
    {
        this.itemName = name;
        this.amount = amount;
        this.type = type;
    }

    public void SetInfo(Item it)
    {
        this.itemName = it.name;
        this.amount = it.amount;
        this.type = it.type;
    }

    public void SetInfo(Item it, int AmountOverride)
    {
        this.itemName = it.name;
        this.amount = AmountOverride;
        this.type = it.type;
    }

    public void SetInfo(int AmountOverride)
    {
        this.amount = AmountOverride;
    }

    void Start()
    {
        if(itemName == "")
            itemName = "Item " + this.gameObject.GetInstanceID().ToString();
    }
}
