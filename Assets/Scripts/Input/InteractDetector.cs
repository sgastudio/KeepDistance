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
    public List<GameObject> interactableList;
    bool interactCooldown = false;
    float interactCooldownTime = 1f;
    float LastInteractTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (input.interactAxis > 0 && interactCooldown == false)
        {
            interactCooldown = true;
            if (interactableList.Count > 1)
                interactableList.Sort(compareDistance);
            GameObject sceneObj = interactableList[0];
            interactableList.Remove(sceneObj);
            LastInteractTime = Time.time;
            //Add to Inventory
            inventory.AddItem("Test3",ItemType.General,1,sceneObj);
        }

        if(Time.time>LastInteractTime + interactCooldownTime)
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
