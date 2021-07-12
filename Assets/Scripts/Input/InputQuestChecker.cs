using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
public class InputQuestChecker : MonoBehaviour
{
    public List<string> axises;
    public Vector2 axisThreshold = new Vector2(-0.8f, 0.8f);
    [Space]
    public List<KeyCode> keys;
    public Dictionary<string, bool> axisesTable;
    public Dictionary<KeyCode, bool> keysTable;

    [Space]
    public WorkMode mode;
    public float CheckInterval = 1.0f;
    public bool disabledAfterTriggered = true;
    public bool waitForQuestActive = true;
    public bool changeQuestState = true;
    [QuestPopup]
    public string questName;

    [Header("Events")]
    public UnityEvent onAllKeyPressed;
    public UnityEvent onAllAxisReached;
    public UnityEvent onAllRequirementReached;


    // Start is called before the first frame update
    void Start()
    {
        keysTable = new Dictionary<KeyCode, bool>();
        axisesTable = new Dictionary<string, bool>();


        foreach (KeyCode key in keys)
        {
            keysTable.Add(key, false);
        }

        foreach (string axis in axises)
        {
            axisesTable.Add(axis, false);
        }
        StartCoroutine(doCheck());

    }

    void Update()
    {
        if (DialogueManager.isConversationActive || QuestLog.GetQuestState(questName) != QuestState.Active)
            return;
        // bool keyResult = mode == WorkMode.Or ? false : true;
        // bool axisResult = mode == WorkMode.Or ? false : true;

        foreach (KeyCode key in keys)
        {
            bool keyInfo = Input.GetKey(key);

            if (keyInfo)
                keysTable[key] = true;

            // if (mode == WorkMode.Or)
            //     keyResult |= keyInfo;
            // else
            //     keyResult &= keyInfo;
        }

        foreach (string axis in axises)
        {

            float axisInfo = Input.GetAxis(axis);
            bool axisTest = Input.GetAxis(axis) <= axisThreshold.x || Input.GetAxis(axis) >= axisThreshold.y;

            if (axisTest)
                axisesTable[axis] = true;

            // if (mode == WorkMode.Or)
            //     axisResult |= axisTest;
            // else
            //     axisResult &= axisTest;
        }
    }

    IEnumerator doCheck()
    {
        for (; ; )
        {
            yield return new WaitForSecondsRealtime(CheckInterval);
            CheckKeyTable();
        }
    }

    bool CheckKeyTable()
    {
        if (DialogueManager.isConversationActive || QuestLog.GetQuestState(questName) != QuestState.Active)
            return false;

        bool keyResult = mode == WorkMode.Or ? false : true;
        bool axisResult = mode == WorkMode.Or ? false : true;
        foreach (KeyValuePair<KeyCode, bool> pair in keysTable)
        {
            if (mode == WorkMode.Or)
                keyResult |= pair.Value;
            else
                keyResult &= pair.Value;
        }

        foreach (KeyValuePair<string, bool> pair in axisesTable)
        {
            if (mode == WorkMode.Or)
                keyResult |= pair.Value;
            else
                keyResult &= pair.Value;
        }

        bool fullResult = mode == WorkMode.Or ? keyResult || axisResult : keyResult && axisResult;

        if (keyResult)
            onAllKeyPressed.Invoke();
        if (axisResult)
            onAllAxisReached.Invoke();
        if (fullResult)
        {
            if (changeQuestState)
                QuestLog.SetQuestState(questName, QuestState.Success);
            onAllRequirementReached.Invoke();
            if (disabledAfterTriggered)
                this.enabled = false;
            ResetTables();
            //this.gameObject.SetActive(false);
        }

        return fullResult;
    }

    public void ResetTables()
    {
        foreach (KeyCode key in keys)
        {
            keysTable[key] = false;
        }

        foreach (string axis in axises)
        {
            axisesTable[axis] = false;
        }
    }
}
