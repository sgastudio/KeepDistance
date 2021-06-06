using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EnumGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate Tag Enum")]
    public static void GenTagEnum()
    {
        var tags = InternalEditorUtility.tags;
        var arg = "";
        foreach (var tag in tags)
        {
            arg += "\t" + tag + ",\n";
        }
        var res = "public enum EnumTag\n{\n" + arg + "}\n";
        var path = Application.dataPath + "/Scripts/Inspector/EnumTag.cs";
        File.WriteAllText(path, res, Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Generate Level Enum")]
    public static void GenLevelEnum()
    {
        var scenes = EditorBuildSettings.scenes;
        var arg = "";
        foreach (var scene in scenes)
        {
            arg += "\t" + scene.path.Substring(scene.path.LastIndexOf('/') + 1, scene.path.LastIndexOf('.') - scene.path.LastIndexOf('/') - 1) + ",\n";
        }
        var res = "public enum EnumLevel\n{\n" + arg + "}\n";
        var path = Application.dataPath + "/Scripts/Inspector/EnumLevel.cs";
        File.WriteAllText(path, res, Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}