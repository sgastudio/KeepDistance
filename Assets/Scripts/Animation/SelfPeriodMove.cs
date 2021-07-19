using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfPeriodMove : MonoBehaviour
{
    public bool useFixedUpdate=true;
    public Space moveSpace;
    public float period = 1;
    float anglePeriod = 0;
    public Vector3 moveSpeed = Vector3.zero;
    public FuncType calculateFunction = 0;

    public enum FuncType
    {
        sin,
        cos,
    }

    // Update is called once per frame
    void Update()
    {
        if (!useFixedUpdate)
        {
            anglePeriod += Time.deltaTime / period;

            if (calculateFunction == FuncType.sin)
                this.transform.Translate(moveSpeed * Mathf.Cos(anglePeriod) * Time.deltaTime, moveSpace);
            else
                this.transform.Translate(moveSpeed * Mathf.Sin(anglePeriod) * Time.deltaTime, moveSpace);
        }

    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            anglePeriod += Time.deltaTime / period;

            if (calculateFunction == FuncType.sin)
                this.transform.Translate(moveSpeed * Mathf.Cos(anglePeriod) * Time.deltaTime, moveSpace);
            else
                this.transform.Translate(moveSpeed * Mathf.Sin(anglePeriod) * Time.deltaTime, moveSpace);
        }
    }
}
