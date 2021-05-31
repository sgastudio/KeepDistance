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

    // Update is called once per frame
    void Update()
    {
        if (input.interactAxis > 0 && interactCooldown == false && activeList.Count > 0)
        {
            interactCooldown = true;
            if (activeList.Count > 1)
                activeList.Sort(compareDistance);
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
                activeList.Remove(sceneObj);
            }
            //Switch
            else if (sceneObj.tag == switchTag.ToString())
            {
                SwitchAgent agent = sceneObj.GetComponent<SwitchAgent>();
                if(agent)
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

        return (int)(dstx - dsty);
    }
}
