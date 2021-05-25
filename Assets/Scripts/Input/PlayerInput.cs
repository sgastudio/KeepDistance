using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string jumpAxisName = "Jump";
    [Space]
    public Vector2 positionAxisWeight = Vector2.one;
    public Vector2 rotationAxisWeight =new Vector2(1,0);
    [ROA,Space]
    public Vector2 positionAxis;
    [ROA]
    public Vector2 rotationAxis;
    [ROA]
    public float jumpAxis;
    [Header("Movement")]
    public float positionVelocity = 2.5f;
    public float rotationVelocity = 10.0f;


    void OnEnable()
    {
        Cursor.lockState = cursorLock;
        Cursor.visible = cursorVisible;
    }

    void FixedUpdate()
    {

        Vector3 desiredVelocity = new Vector3(positionAxis.x * positionAxisWeight.x, 0, positionAxis.y * positionAxisWeight.y);

        Vector3 caculatedVelocity = this.transform.rotation * desiredVelocity * positionVelocity;
        this.transform.Translate(caculatedVelocity * Time.deltaTime, Space.World);

        /*
        Vector3 caculatedVelocity = desiredVelocity * 5.0f;
        this.transform.Translate(caculatedVelocity * Time.deltaTime, Space.Self);
        */

        
        Vector3 desiredAngularVelocity = new Vector3(rotationAxis.y * rotationAxisWeight.y * aspectRate, rotationAxis.x * rotationAxisWeight.x, 0);
        Vector3 caculatedAngularVelocity = desiredAngularVelocity * rotationVelocity * Mathf.Rad2Deg;
        this.transform.rotation *= Quaternion.Euler(caculatedAngularVelocity * Time.deltaTime);


    }

    void Update()
    {
        positionAxis = new Vector2(Input.GetAxis(positionXAxisName), Input.GetAxis(positionYAxisName));
        rotationAxis = new Vector2(Input.GetAxis(rotationXAxisName), Input.GetAxis(rotationYAxisName));
        jumpAxis = Input.GetAxis("Jump");
    }
}
