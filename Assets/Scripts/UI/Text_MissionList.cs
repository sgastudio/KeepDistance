using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Text_MissionList : MonoBehaviour
{
    Text textElement;
    MissionManager missionManager;

    void Awake()
    {
        textElement = this.GetComponent<Text>();
        missionManager = GameObject.FindWithTag(EnumTag.GameController.ToString()).GetComponent<MissionManager>();
        PrintText();
    }

    // Update is called once per frame
    void Update()
    {
        PrintText();
    }

    void PrintText()
    {
        if (textElement && missionManager)
        {
            textElement.text = "Missions\n";
            foreach (MissionContent m in missionManager.missionList)
            {
                textElement.text += (m.isFinished ? "√" : "×") + m.name + (string.IsNullOrWhiteSpace(m.description) ? "" : "\n" + m.description) + "\n\n";
            }
        }
    }
}
