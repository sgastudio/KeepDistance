using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Line : MonoBehaviour
{
    //[RequireComponent(typeof(LineRenderer))]
    LineRenderer line;
    public GameObject target;
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
            line.positionCount = 2;
            Vector3 direction = target.transform.position - this.transform.position;
            line.SetPosition(0,this.transform.position + direction * postionOffset.x);
            line.SetPosition(1,target.transform.position - direction * postionOffset.y);
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
