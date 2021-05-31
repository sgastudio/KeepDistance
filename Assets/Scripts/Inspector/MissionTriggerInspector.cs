using System;
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
    /*public SerializedProperty targetObj;
    public SerializedProperty targetOffset;
    public SerializedProperty followRotation;
    public SerializedProperty snapObj;
    public SerializedProperty snapPosOffset;
    public SerializedProperty snapRotOffset;
    public SerializedProperty snapWeight;
    public SerializedProperty rawDistance;
    public SerializedProperty rawAngle;
    public SerializedProperty gizmosSize;
    public SerializedProperty switchRaycast;
    public SerializedProperty switchPosSlerp;
    public SerializedProperty switchRotSlerp;
    public SerializedProperty ParamPosSlerp;
    public SerializedProperty ParamRotSlerp;*/

    public SerializedProperty missionName;
    
    public void OnEnable()
    {
        obj = new SerializedObject(target);
        missionManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<MissionManager>();

        missionName = obj.FindProperty("missionName");
        /*targetObj = obj.FindProperty("targetObject");
        snapObj = obj.FindProperty("snapObject");

        targetOffset = obj.FindProperty("targetPositionOffset");
        snapPosOffset = obj.FindProperty("snapPositionOffset");
        snapRotOffset = obj.FindProperty("snapRotationOffset");

        followRotation = obj.FindProperty("followTargetRotation");
        rawDistance = obj.FindProperty("cameraDistance");
        rawAngle = obj.FindProperty("cameraAngle");

        snapWeight = obj.FindProperty("snapWeight");

        switchRaycast = obj.FindProperty("enableRaycastDetection");
        switchPosSlerp = obj.FindProperty("enablePositionSlerp");
        switchRotSlerp = obj.FindProperty("enableRotationSlerp");

        ParamPosSlerp = obj.FindProperty("positionSlerpParam");
        ParamRotSlerp = obj.FindProperty("rotationSlerpParam");

        gizmosSize = obj.FindProperty("gizmosSize");*/
    }
    public override void OnInspectorGUI()
    {
        obj.Update();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Target Mission");
        string[] arr = missionManager.GetMissionArray();
        this.missionName.stringValue = arr[EditorGUILayout.Popup(missionManager.FindMissionIndex(missionName.stringValue), arr)];
        GUILayout.EndHorizontal();
        obj.ApplyModifiedProperties();
    }
}
#endif