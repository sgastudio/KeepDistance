using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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