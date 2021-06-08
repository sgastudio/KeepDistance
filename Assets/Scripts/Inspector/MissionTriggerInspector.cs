/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(MissionTrigger))]
public class MissionTriggerInspector : Editor
{
    SerializedObject obj;
    MissionManager missionManager;

    public SerializedProperty missionName;

    public void OnEnable()
    {
        obj = new SerializedObject(target);
        missionManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<MissionManager>();
        missionName = obj.FindProperty("missionName");
    }
    public override void OnInspectorGUI()
    {
        obj.Update();
        GUILayout.BeginHorizontal();
        string[] arr = missionManager.GetMissionArray();
        int index = EditorGUILayout.Popup("Target Mission",missionManager.FindMissionIndex(missionName.stringValue), arr);
        this.missionName.stringValue = arr[index >= 0 ? index : 0];
        GUILayout.EndHorizontal();
        obj.ApplyModifiedProperties();
    }
}
#endif*/