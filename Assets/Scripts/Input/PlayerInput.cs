using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using PixelCrushers.DialogueSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Mouse")]
    public CursorLockMode cursorLock = CursorLockMode.Confined;
    public bool cursorVisible = true;
    public float aspectRate = 16 / 9;
    [Header("Axis")]
    public string positionXAxisName = "Horizontal";
    public string positionYAxisName = "Vertical";
    [Space]
    public string rotationXAxisName = "Mouse X";
    public string rotationYAxisName = "Mouse Y";
    [Space]
    public Vector2 positionAxisWeight = Vector2.one;
    public Vector2 rotationAxisWeight = new Vector2(1, 0);
    [Space]
    public string jumpAxisName = "Jump";
    public string fire1AxisName = "Fire1";
    public string fire2AxisName = "Fire2";
    public string interactAxisName = "Fire3";
    [ROA, Header("Monitor")]
    public Vector2 positionAxis;
    [ROA]
    public Vector2 rotationAxis;
    [ROA]
    public float jumpAxis;
    [ROA]
    public float fire1Axis;
    [ROA]
    public float fire2Axis;
    [ROA]
    public float interactAxis;
    [Header("Movement")]
    public float positionVelocity = 2.5f;
    public float rotationVelocity = 10.0f;
    [ROA]
    public Vector3 adjustedVelocity;
    [Header("Adjustment")]
    public LineDetector AdjustObjects;
    [Range(0, 1)]
    public float AdjustWeight = 0.8f;
    [ROA]
    public float AdjustValue;

    #region private variables
    Vector3 previousDirection;
    PhotonView photonView;
    #endregion

    void Start()
    {
        Cursor.lockState = cursorLock;
        Cursor.visible = cursorVisible;

        photonView = this.GetComponent<PhotonView>();
    }

    
    void FixedUpdate()
    {
        if ((photonView.IsMine == false && PhotonNetwork.IsConnected == true) || (DialogueManager.isConversationActive))// && !(bool)DialogueManager.instance.masterDatabase.GetVariable("MoveState").InitialBoolValue))
            return;
        Vector3 desiredVelocity = new Vector3(positionAxis.x * positionAxisWeight.x, 0, positionAxis.y * positionAxisWeight.y);


        if (AdjustObjects)
        {
            Vector3 direction = (this.transform.rotation * desiredVelocity).normalized;
            if (direction.magnitude == 0)
                direction = previousDirection;
            else
                previousDirection = direction;
            AdjustValue = 1.0f;
            foreach (GameObject o in AdjustObjects.activeList)
            {
                float projection = Vector3.Dot(o.transform.position - this.transform.position, direction);
                float percentage = (projection > 0 ? projection / AdjustObjects.distance : 0) * AdjustWeight;
                AdjustValue *= (1f - percentage);
            }
        }
        adjustedVelocity = desiredVelocity * AdjustValue;
        Vector3 caculatedVelocity = this.transform.rotation * adjustedVelocity * positionVelocity;
        this.transform.Translate(caculatedVelocity * Time.deltaTime, Space.World);

        /*
        Vector3 caculatedVelocity = desiredVelocity * 5.0f;
        this.transform.Translate(caculatedVelocity * Time.deltaTime, Space.Self);
        */

        Vector3 desiredAngularVelocity = new Vector3(rotationAxis.y * rotationAxisWeight.y, rotationAxis.x * rotationAxisWeight.x / aspectRate, 0);
        Vector3 caculatedAngularVelocity = desiredAngularVelocity * rotationVelocity * Mathf.Rad2Deg;
        this.transform.rotation *= Quaternion.Euler(caculatedAngularVelocity * Time.deltaTime);
    }

    void Update()
    {
        if ((photonView.IsMine == false && PhotonNetwork.IsConnected == true) || (DialogueManager.isConversationActive))// && !(bool)DialogueManager.instance.masterDatabase.GetVariable("CameraState").InitialBoolValue))
            return;
        positionAxis = new Vector2(Input.GetAxis(positionXAxisName), Input.GetAxis(positionYAxisName));
        rotationAxis = new Vector2(Input.GetAxis(rotationXAxisName), Input.GetAxis(rotationYAxisName));
        jumpAxis = Input.GetAxis(jumpAxisName);
        fire1Axis = Input.GetAxis(fire1AxisName);
        fire2Axis = Input.GetAxis(fire2AxisName);
        interactAxis = Input.GetAxis(interactAxisName);
    }
}
