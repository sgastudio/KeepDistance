using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class InventoryManager : Inventory
{
    public UnityEvent<string> onItemEquipped;

    [Header("Items")]
    public List<Item> items;
    public List<MountPoint> mountPoints;

    public MountPoint FindMountPoint(string pointName)
    {
        return mountPoints[FindMountPointIndex(pointName)];
    }

    public int FindMountPointIndex(string pointName)
    {
        return mountPoints.FindIndex(result =>
        {
            return result.name == pointName;
        });
    }

    public MountPoint FindMountPoint(ItemStatus pointType)
    {
        return mountPoints[FindMountPointIndex(pointType)];
    }

    public int FindMountPointIndex(ItemStatus pointType)
    {
        return mountPoints.FindIndex(result =>
        {
            return result.type == pointType;
        });
    }

    public Item FindItemAgent(string itemName)
    {
        return items[FindItemIndex(itemName)];
    }

    public override int FindItem(string itemName)
    {
        return FindItemIndex(itemName);
    }

    public int FindItemIndex(string itemName)
    {
        return items.FindIndex(result =>
        {
            return result.name == itemName;
        });
    }

    public Item FindMountedItem(ItemStatus mountPoint)
    {
        return items[FindMountedItemIndex(mountPoint)];
    }

    public int FindMountedItemIndex(ItemStatus mountPoint)
    {
        return items.FindIndex(result =>
        {
            return result.status == mountPoint;
        });
    }

    public override void AddItem(ItemAgent other)
    {
        AddItem(other.name,other.type,other.amount,other.gameObject);
        base.AddItem(other);
    }

    public void AddItem(string itemName, ItemType type = ItemType.Unknown, int addCount = 1, GameObject prefab = null)
    {
        if (FindItemIndex(itemName) >= 0)
        {
            FindItemAgent(itemName).amount += addCount;
            if (prefab)
                GameObject.Destroy(prefab);
        }
        else
        {
            if (prefab && prefab.scene.isLoaded)
            {
                prefab.SetActive(false);
                prefab.transform.SetParent(this.transform);
                prefab.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
                if(prefab.GetComponent<Rigidbody>())
                {
                    prefab.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            items.Add(new Item(itemName, type, addCount, prefab));
        }
    }

    public void EquipItemBack(int itemIndex)
    {
        EquipItem(itemIndex, ItemStatus.Back);
    }
    public void EquipItemBack(string itemName)
    {
        EquipItemBack(FindItemIndex(itemName));
    }
    public void EquipItemRHand(int itemIndex)
    {
        EquipItem(itemIndex, ItemStatus.RightHand);
    }
    public void EquipItemRHand(string itemName)
    {
        EquipItemRHand(FindItemIndex(itemName));
    }
    public void EquipItemLHand(int itemIndex)
    {
        EquipItem(itemIndex, ItemStatus.LeftHand);
    }
    public void EquipItemLHand(string itemName)
    {
        EquipItemLHand(FindItemIndex(itemName));
    }

    public void EquipItem(string itemName, ItemStatus mountPoint)
    {
        EquipItem(FindItemIndex(itemName), mountPoint);
    }
    public void EquipItem(int itemIndex, ItemStatus mountPoint)
    {
        if (itemIndex >= 0)
        {
            if(items[itemIndex].status == mountPoint)
                return;
            UnequipAnyItem(mountPoint);
            if (items[itemIndex].prefab)
                GameObject.Instantiate(items[itemIndex].prefab, FindMountPoint(mountPoint).anchorTransform).SetActive(true);
            items[itemIndex].status = mountPoint;
            onItemEquipped.Invoke(items[itemIndex].name);
        }
    }

    public void UnequipItem(string itemName)
    {
        UnequipItem(FindItemIndex(itemName));
    }

    public void UnequipItem(int itemIndex)
    {
        if (itemIndex >= 0)
        {
            int mpIndex = FindMountPointIndex(items[itemIndex].status);
            if (mountPoints[mpIndex].anchorTransform.childCount > 0)
                GameObject.Destroy(mountPoints[mpIndex].anchorTransform.GetChild(0).gameObject);
            items[itemIndex].status = ItemStatus.Backpack;
        }
    }

    public void UnequipAnyItem(ItemStatus mountPoint)
    {
        int itemIndex = FindMountedItemIndex(mountPoint);
        if (itemIndex >= 0)
        {
            UnequipItem(itemIndex);
        }
    }

    public void RemoveItem(string itemName)
    {
        DropItem(itemName, int.MaxValue);
    }

    public void DropItemOnce(string itemName)
    {
        DropItemOnce(FindItemIndex(itemName));
    }
    public void DropItemOnce(int itemIndex)
    {
        DropItem(itemIndex, 1);
    }
    public void DropItem(string itemName, int count = 1)
    {
        DropItem(FindItemIndex(itemName), count);
    }

    public override void DropItem(ItemAgent other)
    {
        DropItem(other.name,other.amount);
        base.DropItem(other);
    }

    public void DropItem(int itemIndex, int count = 1)
    {
        //check item exist
        if (itemIndex < 0)
            return;

        //drop an instance
        if (items[itemIndex].amount > count)
        {// drop some of the item
            items[itemIndex].amount -= count;

            if (items[itemIndex].prefab)
            {//if the item have prefab/sceneObject
                GameObject sceneObject = GameObject.Instantiate(items[itemIndex].prefab, this.transform.position, this.transform.rotation);
                sceneObject.SetActive(true);
                //if the prefab/sceneobject have Item Agent Component
                SetAgentInfo(sceneObject, items[itemIndex], count);
                if(sceneObject.GetComponent<Rigidbody>())
                {
                    sceneObject.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            else
            {//when there is no prefab or scene object of the item
                CreateEmptyItemObject(items[itemIndex]);
            }
            
            if(items[itemIndex].status!=ItemStatus.Backpack)
            {
                ItemAgent childAgent = mountPoints[FindMountPointIndex(items[itemIndex].status)].anchorTransform.GetChild(0).GetComponent<ItemAgent>();
                childAgent.amount = items[itemIndex].amount;
            }

            onItemDropped.Invoke(items[itemIndex].name);
        }
        else
        {//all of the item will be droped

            if (items[itemIndex].prefab)
            {//if the item have prefab/sceneObject
                GameObject sceneObject;
                if (items[itemIndex].prefab.scene.isLoaded)
                {
                    items[itemIndex].prefab.SetActive(true);
                    items[itemIndex].prefab.transform.SetParent(null);
                    sceneObject = items[itemIndex].prefab;
                }
                else
                {
                    sceneObject = GameObject.Instantiate(items[itemIndex].prefab, this.transform.position, this.transform.rotation);
                }

                SetAgentInfo(sceneObject, items[itemIndex], items[itemIndex].amount);

                if(sceneObject.GetComponent<Rigidbody>())
                {
                    sceneObject.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            else
            {//when there is no prefab or scene object of the item
                CreateEmptyItemObject(items[itemIndex]);
            }

            //optional operations if items are mounted on any points
            if (items[itemIndex].status > 0)
                UnequipItem(itemIndex);
            onItemDropped.Invoke(items[itemIndex].name);
            items.RemoveAt(itemIndex);
        }

    }

    public GameObject CreateEmptyItemObject(Item item)
    {
        GameObject emptyObject = new GameObject(item.name);
        emptyObject.tag = "Item";
        emptyObject.transform.SetPositionAndRotation(transform.position, this.transform.rotation);
        emptyObject.AddComponent<ItemAgent>().SetInfo(item, item.amount);
        emptyObject.AddComponent<BoxCollider>();
        return emptyObject;
    }

    public void SetAgentInfo(GameObject sceneObject, Item item, int amountOverride)
    {
        if (sceneObject.GetComponent<ItemAgent>())
            sceneObject.GetComponent<ItemAgent>().SetInfo(item, amountOverride);
        else
            sceneObject.AddComponent<ItemAgent>().SetInfo(item, amountOverride);
    }
}
