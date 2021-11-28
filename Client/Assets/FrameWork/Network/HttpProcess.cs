using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpProcess
{
    static IEnumerator sendRequest(UnityWebRequest request, Action<int, string> back)
    {
        if (!NetAvailable)
        {
            back?.Invoke(-1, "网络错误");
            yield break;
        }

        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            if (request.downloadHandler != null && !string.IsNullOrEmpty(request.downloadHandler.text))
            {
                back?.Invoke(0, request.downloadHandler.text);
            }
            else
                back?.Invoke(1, request.error);
        }
        else
        {
            string data = request.downloadHandler.text;
            back?.Invoke(0, data);
            SQDebug.Log("Post返回数据:" + data);
        }
        yield return null;
    }

    /// <summary>  
    /// 网络可用否
    /// </summary>  
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }



    public static void SendPost(string url, Action<int, string> back)
    {
        Debug.Log("URL=" + url);

        UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        request.timeout = 10;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        GameManager.Instance.StartCoroutine(sendRequest(request, back));
    }
}
