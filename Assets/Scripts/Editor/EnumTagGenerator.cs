using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EnumTagGenerator : MonoBehaviour
{
    [MenuItem("Tools/GenTagEnum")]
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
}