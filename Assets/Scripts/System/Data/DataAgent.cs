using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAgent : MonoBehaviour
{
    public List<Data> dataStrip;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.AddRangeData(dataStrip);
    }
}
