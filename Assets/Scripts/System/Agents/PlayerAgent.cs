using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerAgent : MonoBehaviour
{
    public string playerName;
    public int playerGroup;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(playerName))
            playerName = this.gameObject.name + this.gameObject.GetHashCode().ToString();
        photonView = this.GetComponent<PhotonView>();
        BindCamera();
    }

    void BindCamera()
    {
        CameraOperation camera = Camera.main.GetComponent<CameraOperation>();

        if (camera)
        {
            if(photonView.IsMine)
                camera.TrackingObject(this.gameObject);
        }
        else
        {
            Debug.LogError("PlayerInput Trying to bind camera, but missing CameraOperation script on main camera");
        }
    }

}
