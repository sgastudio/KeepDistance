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
    public SerializedProperty followRotation;
    public SerializedProperty snapObj;
    public SerializedProperty snapPosOffset;
    public SerializedProperty snapRotOffset;
    public SerializedProperty snapWeight;
    public SerializedProperty rawDistance;
    public SerializedProperty rawAngle;
    public SerializedProperty gizmosSize;
    public SerializedProperty switchRaycast;
    public SerializedProperty ParamRayLayer;
    public SerializedProperty ParamMinDistance;
    public SerializedProperty switchPosSlerp;

    public SerializedProperty ParamPosSlerp;
    public SerializedProperty switchRotSlerp;
    public SerializedProperty ParamRotSlerp;
    public void OnEnable()
    {
        obj = new SerializedObject(target);

        targetObj = obj.FindProperty("targetObject");
        snapObj = obj.FindProperty("snapObject");

        targetOffset = obj.FindProperty("targetPositionOffset");
        snapPosOffset = obj.FindProperty("snapPositionOffset");
        snapRotOffset = obj.FindProperty("snapRotationOffset");

        followRotation = obj.FindProperty("followTargetRotation");
        rawDistance = obj.FindProperty("cameraDistance");
        rawAngle = obj.FindProperty("cameraAngle");

        snapWeight = obj.FindProperty("snapWeight");

        switchRaycast = obj.FindProperty("enableRayDetection");
        ParamRayLayer = obj.FindProperty("rayLayer");
        ParamMinDistance = obj.FindProperty("minCameraDistance");

        switchPosSlerp = obj.FindProperty("enablePositionSlerp");
        ParamPosSlerp = obj.FindProperty("positionSlerpWeight");

        switchRotSlerp = obj.FindProperty("enableRotationSlerp");
        ParamRotSlerp = obj.FindProperty("rotationSlerpWeight");

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
        cameraOperation.followingOnStart = EditorGUILayout.Toggle("Tracking On Start", cameraOperation.followingOnStart);
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
            EditorGUILayout.PropertyField(followRotation);

            EditorGUILayout.PropertyField(switchRaycast);
            if (cameraOperation.enableRayDetection)
            {
                EditorGUILayout.PropertyField(ParamMinDistance);
                EditorGUILayout.PropertyField(ParamRayLayer);
            }

            EditorGUILayout.PropertyField(switchPosSlerp);
            if (cameraOperation.enablePositionSlerp)
                EditorGUILayout.PropertyField(ParamPosSlerp);

            EditorGUILayout.PropertyField(switchRotSlerp);
            if (cameraOperation.enableRotationSlerp)
                EditorGUILayout.PropertyField(ParamRotSlerp);


            //Debug.Log("TPS");
        }

        EditorGUILayout.PropertyField(gizmosSize);

        obj.ApplyModifiedProperties();
    }
}
#endif