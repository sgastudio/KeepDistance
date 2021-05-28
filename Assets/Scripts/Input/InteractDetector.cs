using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryManager inventory;
    public PlayerInput input;
    public string itemTag = "Item";
    public string switchTag = "Switch";
    public LayerMask mask;
    public float interactDelay = 0.5f;
    [ROA]
    public List<GameObject> interactableList;
    bool interactCooldown = false;
    float LastInteractTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (input.interactAxis > 0 && interactCooldown == false && interactableList.Count > 0)
        {
            interactCooldown = true;
            if (interactableList.Count > 1)
                interactableList.Sort(compareDistance);
            GameObject sceneObj = interactableList[0];
            LastInteractTime = Time.time;

            //Add to Inventory
            if (sceneObj.tag == itemTag)
            {
                ItemAgent agent = sceneObj.GetComponent<ItemAgent>();
                if (agent)
                    inventory.AddItem(agent.itemName, agent.type, agent.amount, sceneObj);
                else
                    inventory.AddItem("Item " + sceneObj.GetInstanceID().ToString(), ItemType.Unknown, 1, sceneObj);
                interactableList.Remove(sceneObj);
            }
            //Switch
            else if (sceneObj.tag == switchTag)
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

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag(itemTag) || other.CompareTag(switchTag)) || (mask & other.gameObject.layer) != 0)
        {
            interactableList.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag(itemTag) || other.CompareTag(switchTag)) || (mask & other.gameObject.layer) != 0)
        {
            interactableList.Remove(other.gameObject);
        }
    }
}
