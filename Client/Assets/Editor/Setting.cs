using UnityEditor;
using UnityEngine;

public class Setting : Editor
{

    [MenuItem("Setting/SetDebugSymbols")]
    public static void SetSQDebug() {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "SQDEBUG");
    }
}
