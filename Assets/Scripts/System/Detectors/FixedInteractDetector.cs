using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PixelCrushers.DialogueSystem;
public class FixedInteractDetector : CollisionDetector//, IPunObservable
{
    // Start is called before the first frame update
    [Header("Components")]
    public Inventory inventory;
    //public PlayerInput input;

    [Header("Input")]
    public float interactDelay = 0.2f;
    bool isFiring;
    bool isDropping;
    bool interactCooldown = false;
    float LastInteractTime;

    GameObject lastOutlinedObject;

    public override void Start()
    {
        base.Start();
        this.targetExit.AddListener(CleanOutline);

        if (!inventory)
            inventory = this.GetComponent<Inventory>();
        if (!inventory)
            Debug.LogWarning(gameObject.ToString() + " Missing Inventory Component");
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
            for (int i = 0; i < activeList.Count; i++)
            {
                if (activeList[i].transform.parent == null)
                {
                    UnOutlinedObject(lastOutlinedObject);
                    OutlinedObject(activeList[i]);
                    lastOutlinedObject = activeList[i];
                    return;
                }
            }
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
        IInteractable interactable = sceneObj.GetComponent<IInteractable>();
        SwitchAgent switchAgent = sceneObj.GetComponent<SwitchAgent>();
        ItemAgent itemAgent = sceneObj.GetComponent<ItemAgent>();
        DialogueSystemTrigger dialogueTrigger = sceneObj.GetComponent<DialogueSystemTrigger>();
        //Add to Inventory
        if (itemAgent)
        {
            //Debug.Log("View ID:"+photonView.ViewID.ToString());
            itemAgent.Attach(photonView.ViewID);
            /*
            if (inventory)
            {
                inventory.AddItem(itemAgent);
                Debug.Log("Item ID:" + itemAgent.GetInstanceID().ToString());
            }*/
            /*else
                inventory.AddItem("Item " + sceneObj.GetInstanceID().ToString(), ItemType.Unknown, 1, sceneObj);*/
            UnOutlinedObject(sceneObj);
            activeList.Remove(sceneObj);
        }
        //Switch
        //else if (switchAgent)
        else if (interactable != null)
        {
            //if (switchAgent)
            interactable.Interact();
            //switchAgent.SwitchOnce();
        }
        else if (dialogueTrigger)
        {
            dialogueTrigger.OnUse(this.transform.parent);
            //dialogueTrigger.Fire(this.transform.parent);
        }
    }

    void processInput()
    {
        if (!GetNetworkingTest())
            return;

        isFiring = Input.GetButtonDown("Fire1") && interactCooldown == false;
        isDropping = Input.GetButtonDown("Fire2") && interactCooldown == false;

        // if (Input.GetButtonDown("Fire1") && !isFiring && interactCooldown == false)
        // {
        //     isFiring = true;
        // }
        // if (Input.GetButtonUp("Fire1") && isFiring)
        //     isFiring = false;

        // if (Input.GetButtonDown("Fire2") && !isDropping && interactCooldown == false)
        // {
        //     isDropping = true;
        // }
        // if (Input.GetButtonUp("Fire2") && isDropping)
        //     isDropping = false;
    }

    void Update()
    {
        UpdateList();
        OutlinedList();

        if (PixelCrushers.DialogueSystem.DialogueManager.isConversationActive)
            return;

        processInput();

        if (isFiring && activeList.Count > 0)// && GetNetworkingTest())
        {
            activateFiring();
            interactObject();
        }

        if (isDropping && GetNetworkingTest())
        {
            if (inventory && inventory.GetItem())
                inventory.GetItem().Detach(photonView.ViewID);
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
        var outline = obj.GetComponent<Outline>();
        if (!outline)
            outline = obj.AddComponent<Outline>();
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


    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     //TODO: might be slow to control in this way
    //     if (stream.IsWriting)
    //     {
    //         stream.SendNext(isFiring);
    //     }
    //     else
    //     {
    //         this.isFiring = (bool)stream.ReceiveNext();
    //         if (isFiring && activeList.Count > 0)
    //         {
    //             activateFiring();
    //             interactObject();
    //         }
    //     }
    // }

    #endregion
}
