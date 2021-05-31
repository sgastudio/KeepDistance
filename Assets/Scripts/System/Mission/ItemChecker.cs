using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemCheckerPair{
    public string name;
    public int count;
}
public class ItemChecker : MonoBehaviour
{
    public List<ItemCheckerPair> requireItems;
    public WorkMode workMode;
    
    public UnityEvent onCheckSuccess;
    public void CheckInventory(Collider player)
    {
        InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
        List<int> itemIndex = new List<int>();
        foreach(ItemCheckerPair i in requireItems)
        {
            itemIndex.Add(inventoryManager.FindItemIndex(i.name));
        }

        bool result = false;

        if(workMode == WorkMode.Or)
        {
            for(int i =0;i<itemIndex.Count;i++)
            {
                result |= requireItems[i].count <= inventoryManager.items[itemIndex[i]].amount;
            }
        }
        else
        {
            result = true;
            for(int i =0;i<itemIndex.Count;i++)
            {
                result &= requireItems[i].count <= inventoryManager.items[itemIndex[i]].amount;
            }
        }
        
        if(result)
            onCheckSuccess.Invoke();
    }
}