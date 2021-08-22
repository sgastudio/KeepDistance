using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public class tree<T>
{
    public T node;
    public T[] next;
}*/

public class PanelControl : MonoBehaviour
{
    //public tree<StackPanel>[] panels;
    public StackPanel[] panels;
    Dictionary<StackPanel, int> panelList;

    List<int> panelOrder = new List<int>();
    int currentPanel;
    static int entryNode;
    // Start is called before the first frame update
    void Start()
    {
        panelList = new Dictionary<StackPanel, int>();
        /*for (int i = 0; i < panels.Length; i++)
        {
            panels[i].node.gameObject.SetActive(false);
            panelList.Add(panels[i].node, i);
        }*/

        if(DataManager.ContainData("entry"))
            entryNode = ((int)DataManager.GetData("entry").data);

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].gameObject.SetActive(false);
            panelList.Add(panels[i], i);
        }

        // if (panels.Length > entryNode)
        // {
        //     panels[entryNode].node.gameObject.SetActive(true);
        //     currentPanel = entryNode;
        // }

        if (panels.Length > entryNode)
        {
            panels[entryNode].gameObject.SetActive(true);
            currentPanel = entryNode;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void JumpPanel(int index)
    {
        // panels[currentPanel].node.gameObject.SetActive(false);
        // panelOrder.Add(currentPanel);
        // currentPanel = index;
        // panels[currentPanel].node.gameObject.SetActive(true);
        panels[currentPanel].gameObject.SetActive(false);
        panelOrder.Add(currentPanel);
        currentPanel = index;
        panels[currentPanel].gameObject.SetActive(true);
    }
    
    /*
    public void NextPanel(int index = 0)
    {

        // if (panels[currentPanel].next.Length > index)
        // {
        //     panelOrder.Add(currentPanel);
        //     panels[currentPanel].node.gameObject.SetActive(false);
        //     currentPanel = panelList[panels[currentPanel].next[index]];
        //     panels[currentPanel].node.gameObject.SetActive(true);
        // }

        if (panels[currentPanel].nextPanel.Length > index)
        {
            panelOrder.Add(currentPanel);
            panels[currentPanel].gameObject.SetActive(false);
            currentPanel = panelList[panels[currentPanel].nextPanel[index]];
            panels[currentPanel].gameObject.SetActive(true);
        }

    }

    public void LastPanel()
    {
        // panels[currentPanel].node.gameObject.SetActive(false);
        // currentPanel = panelOrder[panelOrder.Count - 1];
        // panels[panelOrder[panelOrder.Count - 1]].node.gameObject.SetActive(true);
        // panelOrder.RemoveAt(panelOrder.Count - 1);

        panels[currentPanel].gameObject.SetActive(false);
        currentPanel = panelOrder[panelOrder.Count - 1];
        panels[panelOrder[panelOrder.Count - 1]].gameObject.SetActive(true);
        panelOrder.RemoveAt(panelOrder.Count - 1);
    }
    */
    public void ChangeEntry(int i = 0)
    {
        entryNode = i;
    }
}
