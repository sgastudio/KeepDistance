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


    public void CheckArea(Collider other)
    {
        int[] itemCount = new int[requireItems.Count];
        foreach (GameObject item in detector.activeList)
        {
            ItemAgent itemAgent = item.GetComponent<ItemAgent>();
            int requireIndex = requireItems.FindIndex(matcher =>
            {
                if (itemAgent)
                    return matcher.name == itemAgent.itemName;
                else
                    return matcher.name == item.name;
            });
            Debug.Log(requireIndex.ToString()+" / ");

            if (requireIndex >= 0)
                if (itemAgent)
                {
                    itemCount[requireIndex] += itemAgent.amount;
                }
                else
                {
                    itemCount[requireIndex] += 1;
                }

            Debug.Log(requireIndex.ToString()+" / "+ itemCount[requireIndex].ToString());

        }

        bool result = false;

        if (workMode == WorkMode.Or)
        {
            for (int i = 0; i < requireItems.Count; i++)
            {
                result |= requireItems[i].count <= itemCount[i];
            }
        }
        else
        {
            result = true;
            for (int i = 0; i < requireItems.Count; i++)
            {
                result &= requireItems[i].count <= itemCount[i];
            }
        }

        if (result)
            onCheckSucceeded.Invoke();
        else
            onCheckFailed.Invoke();
    }
}
