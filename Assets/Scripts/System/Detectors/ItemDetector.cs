using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDetector : MonoBehaviour
{
    public CollisionDetector detector;
    public List<ItemCheckerPair> requireItems;
    public WorkMode workMode;
    public UnityEvent onCheckSucceeded;
    public UnityEvent onCheckFailed;

    void Start()
    {
        this.detector.targetEnter.AddListener(CheckArea);
        this.detector.targetExit.AddListener(CheckArea);
        this.detector.targetClean.AddListener(CheckArea);
    }

    public void CheckArea(Collider other)
    {
        Dictionary<string,int> itemCount = new Dictionary<string, int>();
        itemCount.Clear();
        foreach (GameObject item in detector.activeList)
        {
            ItemAgent itemAgent = item.GetComponent<ItemAgent>();
            if (!itemAgent)
                break;
            // int requireIndex = requireItems.FindIndex(matcher =>
            // {
            //     if (itemAgent)
            //         return matcher.name == itemAgent.itemName;
            //     else
            //         //return matcher.name == item.name;
            //         return false;
            // });

            //if (requireIndex >= 0)
                //if (itemAgent)
                //{
                if(itemCount.ContainsKey(itemAgent.itemName))
                    itemCount[itemAgent.itemName] += itemAgent.amount;
                else
                    itemCount.Add(itemAgent.itemName, itemAgent.amount);
            //}
            /*else
            {
                itemCount[requireIndex] += 1;
            }*/

            Debug.Log(itemAgent.itemName.ToString() + " contains " + itemCount[itemAgent.itemName].ToString());

        }

        bool result = false;

        if (workMode == WorkMode.Or)
        {
            for (int i = 0; i < requireItems.Count; i++)
            {
                if(itemCount.ContainsKey(requireItems[i].name))
                    result |= requireItems[i].count <= itemCount[requireItems[i].name];
                else
                    result |= false;
            }
        }
        else
        {
            result = true;
            for (int i = 0; i < requireItems.Count; i++)
            {
                 if(itemCount.ContainsKey(requireItems[i].name))
                    result &= requireItems[i].count <= itemCount[requireItems[i].name];
                else
                    result &= false;
            }
        }

        if (result)
            onCheckSucceeded.Invoke();
        else
            onCheckFailed.Invoke();
    }
}
