using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ItemAgent : MonoBehaviourPun
{

    public string itemName;
    [Min(1)]
    public int amount = 1;
    public ItemType type;
    //public int lastOwnerID;

    void Start()
    {
         if (itemName == "")
            itemName = "Item " + this.gameObject.GetInstanceID().ToString();

        this.gameObject.name = "Item " + gameObject.GetInstanceID().ToString();

        photonView.OwnershipTransfer = OwnershipOption.Takeover;
    }

    void Update()
    {
        
    }

    public void SetInfo(string name, int amount, ItemType type)
    {
        this.itemName = name;
        this.amount = amount;
        this.type = type;
    }

    public void SetInfo(Item it)
    {
        this.itemName = it.name;
        this.amount = it.amount;
        this.type = it.type;
    }

    public void SetInfo(Item it, int AmountOverride)
    {
        this.itemName = it.name;
        this.amount = AmountOverride;
        this.type = it.type;
    }

    public void SetInfo(int AmountOverride)
    {
        this.amount = AmountOverride;
    }

    [PunRPC]
    void ItemAttach(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        Inventory inventory = playerView.GetComponentInChildren<Inventory>();
        if (playerView && inventory)
        {
            inventory.AddItem(this);
            if(!PhotonNetwork.OfflineMode)
                this.photonView.TransferOwnership(playerView.Owner.ActorNumber);
            //this.photonView.TransferOwnership(playerViewID);
            //lastOwnerID = playerViewID;
        }
    }

    [PunRPC]
    void ItemDetach(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        Inventory inventory = playerView.GetComponentInChildren<Inventory>();
        if (playerView && inventory)
        {
            inventory.DropItem(this, this.amount);
        }
    }

    public void Attach(int playerViewID)
    {
        if (PhotonNetwork.OfflineMode)
            ItemAttach(playerViewID);
        else
            photonView.RPC("ItemAttach", RpcTarget.All, playerViewID);
    }

    public void Detach(int playerViewID)
    {
        if (PhotonNetwork.OfflineMode)
            ItemDetach(playerViewID);
        else
            photonView.RPC("ItemDetach", RpcTarget.All, playerViewID);
    }
}
