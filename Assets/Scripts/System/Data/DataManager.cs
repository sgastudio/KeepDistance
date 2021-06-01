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
     public List<Data> datasets;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void AddData(string tag, float data,string str)
    {
        if (GetData(tag) == null || datasets.Count == 0)
        {
            datasets.Add(new Data(tag, data,str));
        }
    }
    public void SetData(string tag, float data, string str, bool createWhenNoItem = false)
    {
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

    public Data GetData(string tag)
    {
        return datasets.Find(data =>
        {
           return data.name == tag;
        });
    }

    public void RemoveData(string tag)
    {
        datasets.RemoveAt(GetDataIndex(tag));
    }

    public int GetDataIndex(string tag)
    {
        return datasets.FindIndex(data =>
        {
           return data.name == tag;
        });
    }
}
