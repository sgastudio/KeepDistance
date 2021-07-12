using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;
public class QuestTrigger : MonoBehaviour
{
    public bool assignRuntimeQuest;
    [ShowIf("assignRuntimeQuest")]
    public string questTitle;
    [HideIf("assignRuntimeQuest"),QuestPopup]
    public string quest;
    [DisplayAsString]
    public QuestState currentState;
    //DialogueSystemTrigger trigger;
    string questName;
    // Start is called before the first frame update
    void Start()
    {
        // trigger = this.GetComponent<DialogueSystemTrigger>();
        // if (!trigger)
        //     Debug.LogError("Quest Trigger missing component DialogueSystemTrigger");
        
        if(assignRuntimeQuest)
            questName = questTitle;
        else
            questName = quest;
        currentState = GetQuest();
    }

    public void SetQuest(QuestState state)
    {
        QuestLog.SetQuestState(questName, state);
        currentState = GetQuest();
    }

    public QuestState GetQuest()
    {
        return QuestLog.GetQuestState(questName);
    }

    public void SetQuestStart()
    {
        SetQuest(QuestState.Active);
    }

    public void SetQuestComplete()
    {
        if(GetQuest() >= QuestState.Active)
        SetQuest(QuestState.Success);
    }

    public void SetQuestFail()
    {
        if(GetQuest() >= QuestState.Active)
        SetQuest(QuestState.Failure);
    }

    public void SetQuestAbandon()
    {
        SetQuest(QuestState.Abandoned);
    }
}
