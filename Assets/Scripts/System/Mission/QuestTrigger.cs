using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;
using UnityEngine.Events;
public class QuestTrigger : MonoBehaviour
{
    public bool assignRuntimeQuest;
    [ShowIf("assignRuntimeQuest")]
    public string questTitle;
    [HideIf("assignRuntimeQuest"), QuestPopup]
    public string quest;
    //public bool waitForConversation;
    //public bool disableAfterSuccessOrFail;

    [DisplayAsString]
    public QuestState currentState;

    // [FoldoutGroup("Events")]
    // public UnityEvent onQuestSuccess;
    // [FoldoutGroup("Events")]
    // public UnityEvent onQuestFail;
    // [FoldoutGroup("Events")]
    // public UnityEvent onQuestActive;
    //DialogueSystemTrigger trigger;
    string questName;
    // Start is called before the first frame update
    void Start()
    {
        // trigger = this.GetComponent<DialogueSystemTrigger>();
        // if (!trigger)
        //     Debug.LogError("Quest Trigger missing component DialogueSystemTrigger");

        // if (disableAfterSuccessOrFail)
        // {
        //     onQuestSuccess.AddListener(disableScript);
        //     onQuestFail.AddListener(disableScript);
        // }

        if (assignRuntimeQuest)
            questName = questTitle;
        else
            questName = quest;
        currentState = GetQuest();
    }

    void Update()
    {
        //if (waitForConversation && DialogueManager.IsConversationActive)
        //    return;
        // if (GetQuest() == QuestState.Active && currentState != QuestState.Active)
        //     onQuestActive.Invoke();
        //checkEvents();
        //currentState = GetQuest();
    }

    // void checkEvents()
    // {
    //     if (GetQuest() == QuestState.Active && currentState != QuestState.Active)
    //         onQuestActive.Invoke();

    //     if (GetQuest() == QuestState.Success && currentState == QuestState.Active)
    //         onQuestActive.Invoke();    
    //     if (GetQuest() == QuestState.Failure && currentState == QuestState.Active)
    //         onQuestActive.Invoke();
    // }

    public void SetQuest(QuestState state)
    {
        QuestLog.SetQuestState(questName, state);
        //checkEvents();
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
        if (GetQuest() == QuestState.Active)
        {
            SetQuest(QuestState.Success);
            //onQuestSuccess.Invoke();
        }
    }

    public void SetQuestFail()
    {
        if (GetQuest() == QuestState.Active)
        {
            SetQuest(QuestState.Failure);
            //onQuestFail.Invoke();
        }
    }

    public void SetQuestAbandon()
    {
        if (GetQuest() == QuestState.Active)
            SetQuest(QuestState.Abandoned);
    }

    public void SetQuestUnsigned()
    {
        if (GetQuest() >= QuestState.Active)
            SetQuest(QuestState.Unassigned);
    }

    void disableScript()
    {
        this.enabled = false;
    }
}
