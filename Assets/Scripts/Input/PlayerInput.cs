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
    [ROA]
    public Vector2 positionAxis;
    [ROA]
    public Vector2 rotationAxis;

    void OnEnable()
    {
        Cursor.lockState = cursorLock;
        Cursor.visible = cursorVisible;
    }

    void FixedUpdate()
    {   
        
        Vector3 desiredVelocity = new Vector3(positionAxis.x, 0, positionAxis.y);
 
        Vector3 caculatedVelocity = this.transform.rotation * desiredVelocity * 5.0f;
        this.transform.Translate(caculatedVelocity * Time.deltaTime, Space.World);

        /*
        Vector3 caculatedVelocity = desiredVelocity * 5.0f;
        this.transform.Translate(caculatedVelocity * Time.deltaTime, Space.Self);
        */

        this.transform.rotation *= Quaternion.Euler(0, rotationAxis.x, 0);

    }

    void Update()
    {
        positionAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rotationAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

    }
}
