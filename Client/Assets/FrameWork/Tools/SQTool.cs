using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class SQTool
{


    /// <summary>
    /// 设置透明度
    /// </summary>
    /// <param name="gra"></param>
    /// <param name="a"></param>
    public static void SetColorA(this Graphic gra, float a) {
        gra.color = new Color(gra.color.r, gra.color.g, gra.color.b, a);
    }


    public static string GetPostUrl<T>(this string url, string func, string head, T t) where T : class {
        MsgData data = new MsgData();
        data.msgdata = JsonConvert.SerializeObject(t).Replace("\"", "\"");
        data.msghead = head;

        return string.Format("{0}/{1}?msgdata={2}", url, func, UnityWebRequest.EscapeURL(JsonConvert.SerializeObject(data)));
    }
}
