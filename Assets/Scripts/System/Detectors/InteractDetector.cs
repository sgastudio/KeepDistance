using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetector : CollisionDetector
{
    // Start is called before the first frame update
    [Header("Components")]
    public InventoryManager inventory;
    public PlayerInput input;
    public EnumTag itemTag;
    public EnumTag switchTag;

    [Header("Input")]
    public float interactDelay = 0.5f;
    bool interactCooldown = false;
    float LastInteractTime;

    GameObject lastOutlinedObject;

    void Start()
    {
        this.targetExit.AddListener(CleanOutline);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeList.Count > 1)
            activeList.Sort(compareDistance);
        if (activeList.Count > 0 && activeList[0] != lastOutlinedObject)
        {
            UnOutlinedObject(lastOutlinedObject);
            OutlinedObject(activeList[0]);
            lastOutlinedObject = activeList[0];
        }

        if (input.interactAxis > 0 && interactCooldown == false && activeList.Count > 0)
        {
            interactCooldown = true;
            GameObject sceneObj = activeList[0];
            LastInteractTime = Time.time;

            //Add to Inventory
            if (sceneObj.tag == itemTag.ToString())
            {
                ItemAgent agent = sceneObj.GetComponent<ItemAgent>();
                if (agent)
                    inventory.AddItem(agent.itemName, agent.type, agent.amount, sceneObj);
                else
                    inventory.AddItem("Item " + sceneObj.GetInstanceID().ToString(), ItemType.Unknown, 1, sceneObj);
                UnOutlinedObject(sceneObj);
                activeList.Remove(sceneObj);
            }
            //Switch
            else if (sceneObj.tag == switchTag.ToString())
            {
                SwitchAgent agent = sceneObj.GetComponent<SwitchAgent>();
                if (agent)
                    agent.SwitchOnce();
            }

        }

        if (Time.time > LastInteractTime + interactDelay)
        {
            interactCooldown = false;
        }

        /*if (input.interactAxis == 0 && triggerInteract == true)
            triggerInteract = false;*/
    }

    int compareDistance(GameObject x, GameObject y)
    {
        float dstx = Vector3.Distance(this.transform.position, x.transform.position);
        float dsty = Vector3.Distance(this.transform.position, y.transform.position);

        if (dstx > dsty)
            return 1;
        else if (dstx == dsty)
            return 0;
        else
            return -1;

        //return (int)(dstx - dsty); //inaccuray
    }

    public void OutlinedObject(GameObject obj)
    {
        if (!obj)
            return;
        var outline = obj.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 8f;
    }

    public void UnOutlinedObject(GameObject obj)
    {
        if (!obj)
            return;
        if (lastOutlinedObject == obj)
            lastOutlinedObject = null;
        var outline = obj.GetComponent<Outline>();
        if (outline)
            Destroy(outline);
    }

    public void CleanOutline(Collider other)
    {
        UnOutlinedObject(other.gameObject);
    }
}
