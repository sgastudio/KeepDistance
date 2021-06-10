using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractDetector : CollisionDetector, IPunObservable
{
    // Start is called before the first frame update
    [Header("Components")]
    public InventoryManager inventory;
    public PlayerInput input;

    [Header("Input")]
    public float interactDelay = 0.5f;
    bool isFiring;
    bool interactCooldown = false;
    float LastInteractTime;

    GameObject lastOutlinedObject;

    public override void Start()
    {
        base.Start();
        this.targetExit.AddListener(CleanOutline);
    }

    // Update is called once per frame
    void UpdateList()
    {
        if (activeList.Count > 1)
            activeList.Sort(compareDistance);
    }

    void OutlinedList()
    {
        if (!GetNetworkingTest())
            return;
        if (activeList.Count > 0 && activeList[0] != lastOutlinedObject)
        {
            UnOutlinedObject(lastOutlinedObject);
            OutlinedObject(activeList[0]);
            lastOutlinedObject = activeList[0];
        }
    }
    void activateFiring()
    {
        interactCooldown = true;
        LastInteractTime = Time.time;
    }

    void interactObject()
    {
        GameObject sceneObj = activeList[0];
        SwitchAgent switchAgent = sceneObj.GetComponent<SwitchAgent>();
        ItemAgent itemAgent = sceneObj.GetComponent<ItemAgent>();
        //Add to Inventory
        if (itemAgent)
        {
            if (itemAgent)
                inventory.AddItem(itemAgent.itemName, itemAgent.type, itemAgent.amount, sceneObj);
            else
                inventory.AddItem("Item " + sceneObj.GetInstanceID().ToString(), ItemType.Unknown, 1, sceneObj);
            UnOutlinedObject(sceneObj);
            activeList.Remove(sceneObj);
        }
        //Switch
        else if (switchAgent)
        {
            if (switchAgent)
                switchAgent.SwitchOnce();
        }
    }

    void Update()
    {
        UpdateList();
        OutlinedList();

        isFiring = input.interactAxis > 0 && interactCooldown == false;

        if (isFiring && activeList.Count > 0 && GetNetworkingTest())
        {
            activateFiring();
            interactObject();

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
        if (!obj || !GetNetworkingTest())
            return;
        var outline = obj.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 8f;
    }

    public void UnOutlinedObject(GameObject obj)
    {
        if (!obj || !GetNetworkingTest())
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

    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //TODO: might be slow to control in this way
        if (stream.IsWriting)
        {
            stream.SendNext(isFiring);
        }
        else
        {
            this.isFiring = (bool)stream.ReceiveNext();
            if (isFiring && activeList.Count > 0)
            {
                activateFiring();
                interactObject();
            }
        }
    }


    #endregion
}
