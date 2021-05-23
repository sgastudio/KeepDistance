using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Line : MonoBehaviour
{
    //[RequireComponent(typeof(LineRenderer))]
    LineRenderer line;
    public GameObject target;
    public Vector3 originPosOffset;
    public Vector3 targetPosOffset;
    public Vector2 postionOffset;
    public Vector2 width = new Vector2(1,0);
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    
    // Start is called before the first frame update
    void Start()
    {
       line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(line && target)
        {
            Vector3 originPos = originPosOffset + this.transform.position;
            Vector3 targetPos = targetPosOffset + target.transform.position;

            line.positionCount = 2;
            Vector3 direction = targetPos - originPos;
            line.SetPosition(0,originPos + direction * postionOffset.x);
            line.SetPosition(1,targetPos - direction * postionOffset.y);
            line.startWidth = width.x;
            line.endWidth = width.y;
            line.startColor = startColor;
            line.endColor = endColor;
        }
        else if(!target)
        {
            line.positionCount = 0;
        }
    }
}
