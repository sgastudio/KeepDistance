using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfAroundRotation : MonoBehaviour
{
    public Transform target;
    public Vector3 rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(target.position, target.right, rotateSpeed.x *Time.deltaTime);
        this.transform.RotateAround(target.position, target.up, rotateSpeed.y *Time.deltaTime);
        this.transform.RotateAround(target.position, target.forward, rotateSpeed.z *Time.deltaTime);
    }
}
