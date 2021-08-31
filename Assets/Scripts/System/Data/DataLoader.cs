using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public string fileName;

    public bool readOnStart;

    public bool insertOnStart;
    List<Data> dataStrip;
    // Start is called before the first frame update
    void Start()
    {
        dataStrip = JsonUtility.FromJson<List<Data>>(fileName);

        if(readOnStart)
            ReadFile();

        if(insertOnStart)
            InsertToManager();
    }

    public void WriteFile()
    {
        if(string.IsNullOrEmpty(fileName))
            return;
        string json = JsonUtility.ToJson(dataStrip);
        string fileUrl = Application.streamingAssetsPath + "\\" + fileName;

        StreamWriter sw = new StreamWriter(fileUrl);

        sw.WriteLine(json);
        sw.Close();
    }

    public void ReadFile()
    {
        if(string.IsNullOrEmpty(fileName))
            return;
        string fileUrl = Application.streamingAssetsPath + "\\" + fileName;

        StreamReader sr = File.OpenText(fileUrl);
        string json = sr.ReadToEnd();
        sr.Close();

        dataStrip = JsonUtility.FromJson<List<Data>>(json);
    }

    public void UpdateFromManager()
    {
        for(int i =0;i<dataStrip.Count;i++)
        {
            dataStrip[i] = DataManager.GetData(dataStrip[i].name);
        }
    }

    public void GetFromManager(string tag)
    {
        dataStrip.Add(DataManager.GetData(tag));
    }

    public void InsertToManager()
    {
        DataManager.AddRangeData(dataStrip);
    }
}
