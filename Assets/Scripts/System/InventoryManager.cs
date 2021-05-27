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
    public Item(string name, ItemType type = ItemType.General, int amount = 1, GameObject prefabObj = null)
    {
        this.name = name;
        this.type = type;
        this.amount = amount;
        this.prefab = prefabObj;
    }
    public string name;
    public int amount = 1;
    public ItemType type;
    public ItemStatus status;
    public GameObject prefab;
}

[System.Serializable]
public class MountPoint
{
    public string name;
    public ItemStatus type;
    public Transform anchorTransform;
}

public class InventoryManager : MonoBehaviour
{
    public GameObject testPrefab;
    public GameObject testPrefab2;
    public List<Item> items;
    public List<MountPoint> mountPoints;


    // Start is called before the first frame update
    void Start()
    {
        AddItem("Test", ItemType.General, 2, testPrefab);
        AddItem("Test2", ItemType.General, 1, testPrefab2);
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

    public void AddItem(string itemName, ItemType type = ItemType.Unknown, int addCount = 1, GameObject prefab = null)
    {
        if (FindItemIndex(itemName) >= 0)
        {
            FindItem(itemName).amount += addCount;
        }
        else
        {
            if (prefab && prefab.scene.isLoaded)
            {
                Debug.Log(prefab.GetInstanceID());
                prefab.SetActive(false);
                prefab.transform.SetParent(this.transform);
                prefab.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            }
            items.Add(new Item(itemName, type, addCount, prefab));
        }
    }

    public void EquipItem(string itemName, ItemStatus mountPoint)
    {
        EquipItem(FindItemIndex(itemName), mountPoint);
    }
    public void EquipItem(int itemIndex, ItemStatus mountPoint)
    {
        if (itemIndex >= 0)
        {
            UnequipAnyItem(mountPoint);
            if (items[itemIndex].prefab)
                GameObject.Instantiate(items[itemIndex].prefab, FindMountPoint(mountPoint).anchorTransform).SetActive(true);
            items[itemIndex].status = mountPoint;
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

    public void DropItem(string itemName, int count = 1)
    {
        int itemIndex = FindItemIndex(itemName);
        //check item exist
        if (itemIndex < 0)
            return;
        //drop an instance
        //GameObject.Instantiate(items[itemIndex].prefab, this.transform.position, this.transform.rotation);
        if (items[itemIndex].prefab)
        {
            if (items[itemIndex].prefab.scene.isLoaded)
            {
                items[itemIndex].prefab.SetActive(true);
                items[itemIndex].prefab.transform.SetParent(null);
            }
            else
            {
                GameObject.Instantiate(items[itemIndex].prefab, this.transform.position, this.transform.rotation);
            }
        }
        else
        {//when there is no prefab or scene object of the item
            GameObject emptyObject = new GameObject(items[itemIndex].name);
            emptyObject.tag = "Item";
            emptyObject.transform.SetPositionAndRotation(transform.position, this.transform.rotation);
        }
        
        if (items[itemIndex].amount > count)
        {
            items[itemIndex].amount -= count;
        }
        else
        {
            //optional operations if items are mounted on any points
            if (items[itemIndex].status > 0)
                UnequipItem(itemIndex);
            items.RemoveAt(itemIndex);
        }
    }

    public void TestAdd()
    {
        DropItem("Test2");
    }

    public void TestEquip(string n)
    {
        EquipItem("Test3", ItemStatus.RightHand);
    }
}
