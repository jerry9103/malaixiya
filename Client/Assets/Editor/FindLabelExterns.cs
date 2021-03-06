using UnityEngine;
using UnityEditor;
using System.IO;

public class FindLabelExterns :  MonoBehaviour{
    // 在菜单来创建 选项 ， 点击该选项执行搜索代码    
    [MenuItem("Tools/Check Unity Font")]
    static void Check()
    {
        string[] tmpFilePathArray = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);

        EditorUtility.DisplayProgressBar("CheckUnityFont", "CheckUnityFont", 0f);

        for (int i = 0; i < tmpFilePathArray.Length; i++)
        {
            EditorUtility.DisplayProgressBar("CheckUnityFont", "CheckUnityFont", (i * 1.0f) / tmpFilePathArray.Length);

            string tmpFilePath = tmpFilePathArray[i];

            if (tmpFilePath.EndsWith(".prefab"))
            {
                StreamReader tmpStreamReader = new StreamReader(tmpFilePath);
                string tmpContent = tmpStreamReader.ReadToEnd();
                if (tmpContent.Contains("mFont: {fileID: 0}"))
                {
                    Debug.LogError(tmpFilePath);
                }
            }
        }

        EditorUtility.ClearProgressBar();
    }
}
