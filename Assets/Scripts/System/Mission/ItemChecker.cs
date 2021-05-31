using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour
{
    public string itemName;
    public int itemCount;
    public UnityEvent onCheckSuccess;
    public void CheckInventory(Collider player)
    {
        InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
        int itemIndex = inventoryManager.FindItemIndex(itemName);
        if (inventoryManager &&  itemIndex != -1)
        {
            if(itemCount < inventoryManager.items[itemIndex].amount)
                onCheckSuccess.Invoke();
        }
    }
}
