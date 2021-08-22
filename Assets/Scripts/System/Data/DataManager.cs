using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public Data(string tag, float data, string str)
    {
        this.name = tag;
        this.data = data;
        this.str = str;
    }
    public string name;
    public float data;
    public string str;
}

public class DataManager : MonoBehaviour
{
    public List<Data> dataStrip;
    static public List<Data> datasets;
    // Start is called before the first frame update
    void Awake()
    {
        InitDatasets();
    }

    void Start()
    {
        DataManager.datasets.AddRange(dataStrip);
        DontDestroyOnLoad(this.gameObject);
    }
    
    static public void AddData(string tag, float data,string str)
    {
        InitDatasets();
        if (GetData(tag) == null || datasets.Count == 0)
        {
            datasets.Add(new Data(tag, data,str));
        }
    }

    static public void AddRangeData(IEnumerable<Data> datas)
    {
        InitDatasets();
        datasets.AddRange(datas);
    }

    static public void SetData(string tag, float data, string str, bool createWhenNoItem = false)
    {
        InitDatasets();
        foreach (Data i in datasets)
        {
            if (i.name == tag)
            {
                i.data = data;
                return;
            }
        }

        if (createWhenNoItem)
        {
            AddData(tag, data,str);
        }
    }

    static public Data GetData(string tag)
    {
        InitDatasets();
        return datasets.Find(data =>
        {
           return data.name == tag;
        });
    }

    static public void RemoveData(string tag)
    {
        datasets.RemoveAt(GetDataIndex(tag));
    }

    static public int GetDataIndex(string tag)
    {
        InitDatasets();
        return datasets.FindIndex(data =>
        {
           return data.name == tag;
        });
    }

    static public bool ContainData(string tag)
    {
        InitDatasets();
        return GetDataIndex(tag) >= 0;
    }

    void Update()
    {
        //Debug.Log(ContainData("result")?GetData("result").str:"Data not Avaliable");
    }

    static void InitDatasets()
    {
        if(datasets == null)
            datasets = new List<Data>();
    }
}
