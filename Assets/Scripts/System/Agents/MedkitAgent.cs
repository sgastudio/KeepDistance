using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MedkitAgent : MonoBehaviour,IPunObservable
{
    PhotonView photonView;
    public float rechargeDuration = 2f;
    float lastUsed;
    bool state;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        GetComponent<CollisionDetector>().targetEnter.AddListener(Clean);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastUsed + rechargeDuration)
            SetKitVisiable(true);
    }

    public void Clean(Collider player)
    {
        if (Time.time < lastUsed + rechargeDuration)
            return;
        lastUsed = Time.time;
        SetKitVisiable(false);
        PlayerAgent agent = player.GetComponent<PlayerAgent>();
        if (agent)
        {
            agent.TriggerCleanEffect();
            agent.TriggerVirusEffect(false);
        }
    }

    void SetKitVisiable(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);
        this.state = state;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (photonView)
            if (stream.IsWriting)
            {
                stream.SendNext(this.state);
            }
            else
            {
                SetKitVisiable((bool)stream.ReceiveNext());
            }
    }
}
