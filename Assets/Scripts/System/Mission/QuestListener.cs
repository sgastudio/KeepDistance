using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;
using UnityEngine.Events;
public class QuestListener : MonoBehaviour
{
    public bool assignRuntimeQuest;
    [ShowIf("assignRuntimeQuest")]
    public string questTitle;
    [HideIf("assignRuntimeQuest"), QuestPopup]
    public string quest;
    public bool waitForConversation = true;
    public bool disableAfterSuccessOrFail = true;

    [DisplayAsString]
    public QuestState currentState;

    [FoldoutGroup("Events")]
    public UnityEvent onQuestSuccess;
    [FoldoutGroup("Events")]
    public UnityEvent onQuestFail;
    [FoldoutGroup("Events")]
    public UnityEvent onQuestActive;
    //DialogueSystemTrigger trigger;
    string questName;
    // Start is called before the first frame update
    void Start()
    {
        // trigger = this.GetComponent<DialogueSystemTrigger>();
        // if (!trigger)
        //     Debug.LogError("Quest Trigger missing component DialogueSystemTrigger");

        if (disableAfterSuccessOrFail)
        {
            onQuestSuccess.AddListener(disableScript);
            onQuestFail.AddListener(disableScript);
        }

        if (assignRuntimeQuest)
            questName = questTitle;
        else
            questName = quest;
        currentState = GetQuest();
    }

    void Update()
    {
        if (waitForConversation && DialogueManager.IsConversationActive)
            return;
        checkEvents();
    }

    void checkEvents()
    {
        QuestState tempState = GetQuest();

        if (tempState == QuestState.Active && currentState != QuestState.Active)
            onQuestActive.Invoke();
        if (tempState == QuestState.Success && currentState == QuestState.Active)
            onQuestSuccess.Invoke();    
        if (tempState == QuestState.Failure && currentState == QuestState.Active)
            onQuestFail.Invoke();

        currentState = tempState;
    }


    public QuestState GetQuest()
    {
        return QuestLog.GetQuestState(questName);
    }


    void disableScript()
    {
        this.enabled = false;
    }
}
