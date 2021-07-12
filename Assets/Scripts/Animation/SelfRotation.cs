using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public bool useFixedUpdate=true;
    public Space rotateSpace;
    public Vector3 rotateSpeed = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        if (!useFixedUpdate)
            this.transform.Rotate(rotateSpeed * Time.deltaTime, rotateSpace);
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
            this.transform.Rotate(rotateSpeed * Time.deltaTime, rotateSpace);
    }
}
