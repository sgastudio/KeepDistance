using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemCheckerPair
{
    public string name;
    public int count;
}
public class InventoryChecker : MonoBehaviour
{
    public Inventory inventoryManager;
    public List<ItemCheckerPair> requireItems;
    public WorkMode workMode;
    public UnityEvent onCheckSucceeded;
    public UnityEvent onCheckFailed;
    void Start()
    {
        if (!inventoryManager)
            inventoryManager = this.GetComponent<Inventory>();
        if (inventoryManager)
        {
            inventoryManager.onItemAdded.AddListener(CheckInventory);
            inventoryManager.onItemDropped.AddListener(CheckInventory);
        }
        else
            Debug.LogError("Missing InventoryManager Component in InventoryChecker");
    }
    public void CheckInventory(string ItemName)
    {
        if (!inventoryManager)
            return;

        /*List<int> itemIndex = new List<int>();
        foreach (ItemCheckerPair i in requireItems)
        {
            itemIndex.Add(inventoryManager.FindItemIndex(i.name));
        }*/

        bool result = false;

        if (workMode == WorkMode.Or)
        {
            foreach (ItemCheckerPair i in requireItems)
            {
                result |= inventoryManager.FindItem(i.name) >= i.count;
            }
        }
        else
        {
            result = true;
            foreach (ItemCheckerPair i in requireItems)
            {
                result &= inventoryManager.FindItem(i.name) >= i.count;
            }
        }

        /*if (workMode == WorkMode.Or)
        {
            for (int i = 0; i < itemIndex.Count; i++)
            {
                result |= itemIndex[i] >= 0 ? requireItems[i].count <= inventoryManager.items[itemIndex[i]].amount : false;
            }
        }
        else
        {
            result = true;
            for (int i = 0; i < itemIndex.Count; i++)
            {
                result &= itemIndex[i] >= 0 ? requireItems[i].count <= inventoryManager.items[itemIndex[i]].amount : false;
            }
        }*/

        if (result)
            onCheckSucceeded.Invoke();
        else
            onCheckFailed.Invoke();
    }
}