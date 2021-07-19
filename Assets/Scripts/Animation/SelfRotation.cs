using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public bool useFixedUpdate=true;
    public Space rotateSpace;
    public float period = 1;
    public Vector3 angle = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        if (!useFixedUpdate)
            this.transform.Rotate(angle * Time.deltaTime / period, rotateSpace);
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
            this.transform.Rotate(angle * Time.deltaTime / period, rotateSpace);
    }
}
