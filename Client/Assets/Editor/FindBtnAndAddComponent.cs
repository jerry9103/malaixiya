using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class FindBtnAndAddComponent : EditorWindow
{
    private static string mPath = "";

    [MenuItem("FUnityExtends/FindBtnAndAddComponent")]
    public static void Settings()
    {
        EditorWindow.GetWindow<FindBtnAndAddComponent>(false, "FindBtnAndAddComponent", true);
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("添加CommonBtnClickVoice脚本"))
        {
            mPath = GetReplacePath();
            if (!string.IsNullOrEmpty(mPath))
            {
                AddComponentToBtn();
            }
        }
        GUILayout.EndHorizontal();
    }

    public void AddComponentToBtn()
    {
        List<GameObject> list = GetFiles<GameObject>(mPath, "*.prefab", null);
        if (list == null)
            return;
        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
            Button[] btns = list[i].GetComponentsInChildren<Button>(true);
            if (btns == null)
                continue;
            for (int j = 0; j < btns.Length; j++)
            {
                //if (btns[j].gameObject.GetComponent<CommonBtnClickVoice>() != null)
                //    continue;
                //btns[j].gameObject.AddComponent<CommonBtnClickVoice>();
                Debug.Log("replace " + mPath + "   " + btns[j].gameObject.name);
            }
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("FUnityExtends/FindLabel")]
    public static void FindUILabel()
    {
        mPath = GetReplacePath();

        List<GameObject> list = GetFiles<GameObject>(mPath, "*.prefab", null);
        if (list == null)
            return;
        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
            Text[] labels = list[i].GetComponentsInChildren<Text>(true);
            if (labels == null)
                continue;
            foreach (var label in labels)
            {
                //if (label.trueTypeFont != null && label.trueTypeFont.name.Contains("Lucida"))
                //{
                //    Debug.Log(label.trueTypeFont.name);
                //    Debug.Log("11 " + mPath + "   " + list[i].gameObject.name + label.gameObject.name);
                //}
                //if (label.bitmapFont != null && label.bitmapFont.name.Contains("Lucida"))
                //{
                //    Debug.Log(label.bitmapFont.name);
                //    Debug.Log("22 " + mPath + "   " + list[i].gameObject.name);
                //}

                //if (label.trueTypeFont == null)
                //    Debug.Log("33 " + mPath + "   " + label.gameObject.name);
            }
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }


    public static List<T> GetFiles<T>(string path, string pattern, List<T> outList = null) where T : UnityEngine.Object
    {
        if (outList == null)
            outList = new List<T>();

        string appdata = Application.dataPath;

        string[] filePaths = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
        for (int i = 0; i < filePaths.Length; i++)
        {
            string filePath = "Assets" + filePaths[i].Replace("\\", "/").Replace(appdata, "");
            T obj = (T)AssetDatabase.LoadAssetAtPath(filePath, typeof(T));
            if (obj != null)
                outList.Add(obj);
        }
        return outList;
    }

    private static string GetReplacePath()
    {
        string path = EditorUtility.OpenFolderPanel("选择替换目录", "", "");
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("路径错误");
            return null;
        }
        path += "/";
        return path;
    }
}
