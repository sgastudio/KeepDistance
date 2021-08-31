using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerAgent : MonoBehaviourPun
{
    public string playerName;
    public int playerGroup;
    public int gender;
    public ParticleSystem virusParticle;
    public ParticleSystem cleanParticle;

    public const string PLAYER_GENDER_KEY = "pg";

    public Vector2 spawnOffset;

    public Transform spawnTransform;
    public Vector3 spwanPosition;

    // Start is called before the first frame update
    void Start()
    {
        BindCamera();
    }

    void BindCamera()
    {
        CameraOperation camera = Camera.main.GetComponent<CameraOperation>();

        if (camera)
        {
            if (photonView.IsMine)
                camera.TrackingObject(this.gameObject);
        }
        else
        {
            Debug.LogError("PlayerInput Trying to bind camera, but missing CameraOperation script on main camera");
        }
    }

    void SwitchParticleSystem(ParticleSystem particleSystem, bool state = true)
    {
        if (particleSystem)
            if (state)
                particleSystem.Play();
            else
                particleSystem.Stop();
    }

    [PunRPC]
    void SwitchVirusEffect(bool state)
    {
        SwitchParticleSystem(virusParticle, state);
        if (state)
            GetComponent<PlayerInput>().extraValue = .7f;
        else
            GetComponent<PlayerInput>().extraValue = 1f;
    }

    [PunRPC]
    void SwitchCleanEffect()
    {
        SwitchParticleSystem(cleanParticle, true);
    }


    public void TriggerVirusEffect(bool state)
    {
        if (PhotonNetwork.OfflineMode)
            SwitchVirusEffect(state);
        else
            photonView.RPC("SwitchVirusEffect", RpcTarget.All, state);
    }

    public void TriggerCleanEffect()
    {
        if (PhotonNetwork.OfflineMode)
            SwitchCleanEffect();
        else
            photonView.RPC("SwitchCleanEffect", RpcTarget.All);
    }



    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == EnumTag.Player.ToString() || other.gameObject.tag == EnumTag.NPC.ToString())
        {
            TriggerVirusEffect(true);
            BouncedOff(other);
            DropItemsInInventory();
        }
        else if (other.gameObject.tag == EnumTag.Respawn.ToString())
        {
            RestPostion();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == EnumTag.CheckPoint.ToString())
        {
            if (spawnTransform != other.gameObject.transform)
            {
                spawnTransform = other.gameObject.transform;
                spwanPosition = spawnTransform.position;
                CheckPointAgent agent = other.GetComponent<CheckPointAgent>();
                if(agent)
                    spawnOffset = agent.GetOffset(GetComponent<PhotonView>().OwnerActorNr); 
            }
        }
    }

    void BouncedOff(Collision other, float force = 5f)
    {
        Vector3 target = other.transform.position;
        Vector3 direction = (this.transform.position - target).normalized;
        direction.y = 0;
        float distance = (this.transform.position - target).magnitude;


        GetComponent<PlayerInput>().jumpButton = true;
        Rigidbody body = this.GetComponent<Rigidbody>();
        body.AddForceAtPosition((direction + Vector3.up) * force, direction * distance * .5f, ForceMode.VelocityChange);
        // if(other.rigidbody)
        //     other.rigidbody.AddForceAtPosition((-direction + Vector3.up) * force, direction * distance * .5f, ForceMode.Acceleration);
    }

    void DropItemsInInventory()
    {
        GetComponentInChildren<SimpleInventoryManager>().DropItemAll(null);
    }

    public void RestPostion()
    {
        Vector3 respawnPoint = spwanPosition + new Vector3(spawnOffset.x, 0, spawnOffset.y);
        this.GetComponent<Rigidbody>().position = respawnPoint;
    }

    public void UpdateSpawn(Transform trans)
    {
        spawnTransform = trans;
    }

}
