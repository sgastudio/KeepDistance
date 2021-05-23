using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(CameraOperation))]
public class CameraOperationInspector : Editor
{
    SerializedObject obj;
    public SerializedProperty targetObj;
    public SerializedProperty targetOffset;
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
    public SerializedProperty ParamRotSlerp;
    public void OnEnable()
    {
        obj = new SerializedObject(target);

        targetObj = obj.FindProperty("targetObject");
        snapObj = obj.FindProperty("snapObject");

        targetOffset = obj.FindProperty("targetPositionOffset");
        snapPosOffset = obj.FindProperty("snapPositionOffset");
        snapRotOffset = obj.FindProperty("snapRotationOffset");

        rawDistance = obj.FindProperty("cameraDistance");
        rawAngle = obj.FindProperty("cameraAngle");

        snapWeight = obj.FindProperty("snapWeight");

        switchRaycast = obj.FindProperty("enableRaycastDetection");
        switchPosSlerp = obj.FindProperty("enablePositionSlerp");
        switchRotSlerp = obj.FindProperty("enableRotationSlerp");

        ParamPosSlerp = obj.FindProperty("positionSlerpParam");
        ParamRotSlerp = obj.FindProperty("rotationSlerpParam");

        gizmosSize = obj.FindProperty("gizmosSize");
    }
    public override void OnInspectorGUI()
    {
        //SerializedObject obj = new SerializedObject(target);
        obj.Update();
        //targetObj = obj.FindProperty("targetObject");
        //snapObj = obj.FindProperty("snapObject");

        CameraOperation cameraOperation = target as CameraOperation;
        cameraOperation.mode = (CameraOperation.cameraMode)EditorGUILayout.EnumPopup("Camera Mode", cameraOperation.mode);
        if (cameraOperation.mode == CameraOperation.cameraMode.firstPerson)
        {

            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Label("Targeting", new[] { GUILayout.Height(30) });
            //EditorGUILayout.EndHorizontal();
            /*targetObj = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObj, typeof(GameObject), true);
            cameraOperation.targetObject = targetObj;*/
            EditorGUILayout.PropertyField(snapObj);
            EditorGUILayout.PropertyField(snapPosOffset);
            cameraOperation.useAngleInstead = EditorGUILayout.Toggle("Snap Orientation Offset", cameraOperation.useAngleInstead);
            if (cameraOperation.useAngleInstead)
                EditorGUILayout.PropertyField(snapRotOffset);
            else
                EditorGUILayout.PropertyField(targetOffset);

            
            //Debug.Log("FPS");
        }
        else if (cameraOperation.mode == CameraOperation.cameraMode.thirdPerson)
        {
            /*targetObj = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObj, typeof(GameObject), true);
            cameraOperation.targetObject = targetObj;
            snapObj = (GameObject)EditorGUILayout.ObjectField("Snap Object", snapObj, typeof(GameObject), true);
            cameraOperation.targetObject = snapObj;*/

            EditorGUILayout.PropertyField(snapObj);

            if (cameraOperation.snapObject != null)
            {
                EditorGUILayout.PropertyField(snapPosOffset);
                EditorGUILayout.PropertyField(snapWeight);
                EditorGUILayout.PropertyField(rawDistance);
                EditorGUILayout.PropertyField(rawAngle);
            }
            else
            {
                EditorGUILayout.PropertyField(rawDistance);
                EditorGUILayout.PropertyField(rawAngle);
            }
            EditorGUILayout.PropertyField(targetObj);
            EditorGUILayout.PropertyField(targetOffset);

            EditorGUILayout.PropertyField(switchRaycast);
            EditorGUILayout.PropertyField(switchPosSlerp);

            if(cameraOperation.enablePositionSlerp)
                EditorGUILayout.PropertyField(ParamPosSlerp);
            EditorGUILayout.PropertyField(switchRotSlerp);
            if(cameraOperation.enableRotationSlerp)
                EditorGUILayout.PropertyField(ParamRotSlerp);
            //Debug.Log("TPS");
        }

        EditorGUILayout.PropertyField(gizmosSize);

        obj.ApplyModifiedProperties();
    }
}
#endif