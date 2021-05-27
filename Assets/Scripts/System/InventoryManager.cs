using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Unknown = 0,
    General,
    PowerUps,
    Weapons,
    Gears,
    Mission,
}
public enum ItemStatus
{
    Backpack = 0,
    LeftHand,
    RightHand,
    Back
}

[System.Serializable]
public class Item
{
    public Item(string name, ItemType type = ItemType.General, int amount = 1, GameObject sceneObject = null)
    {
        this.name = name;
        this.amount = amount;
        this.sceneObject = sceneObject;
    }
    public string name;
    public int amount = 1;
    public ItemType type;
    public ItemStatus status;
    public GameObject sceneObject;
}

[System.Serializable]
public class MountPoint
{
    public string name;
    public ItemStatus type;
    public Transform anchorTransforms;
}

public class InventoryManager : MonoBehaviour
{
    public List<Item> items;
    public List<MountPoint> mountPoints;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public Item FindItem(string itemName)
    {
        return items[FindItemIndex(itemName)];
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

    public void AddItem(string itemName, ItemType type = ItemType.Unknown,int addCount=1)
    {
        if (FindItemIndex(itemName) >= 0)
        {
            FindItem(itemName).amount += addCount;
        }
        else
        {
            items.Add(new Item("Test",type,addCount));
        }
    }

    public void RemoveItem(string itemName)
    {
        int itemIndex = FindItemIndex(itemName);
        if (items[itemIndex].amount > 1)
            items[itemIndex].amount -= 1;
        else
        {
            if (items[itemIndex].status > 0 && items[itemIndex].sceneObject)
                GameObject.Destroy(items[itemIndex].sceneObject);
            items.RemoveAt(itemIndex);
        }
    }

    public void TestAdd()
    {
        AddItem("Test", ItemType.General);
    }
}
