using UnityEngine;
using System.Collections;

public class SQDebug : MonoBehaviour
{

    /// <summary>
    /// 打印Log到屏幕
    /// </summary>
    /// <param name="s"></param>
    public static void PrintToScreen(string s)
    {
#if SQDEBUG
        Print(s);
#endif
    }

    /// <summary>
    /// 打印Log到屏幕
    /// </summary>
    /// <param name="objs"></param>
    public static void PrintToScreen(params object[] objs)
    {
#if SQDEBUG
        
#endif
    }


    /// <summary>
    /// 这个就是Debug.Log
    /// </summary>
    /// <param name="message"></param>
    public static void Print(object message)
    {
#if SQDEBUG
        Debug.Log(message);
#endif

    }

    /// <summary>
    /// 这个就是Debug.Log
    /// </summary>
    /// <param name="message"></param>
    public static void Log(object message)
    {
#if SQDEBUG
        if(GameManager.Instance.m_IsShowLog)
        {
            Debug.Log(message);
        }
#endif

    }

    /// <summary>
    /// 打印警告
    /// </summary>
    /// <param name="str"></param>
    public static void LogWarning(string str, Object content = null)
    {
        if (GameManager.Instance.m_IsShowLog)
        {
            Debug.LogWarning(str, content);
        }
    }

    /// <summary>
    /// 打印报错
    /// </summary>
    /// <param name="str"></param>
    public static void LogError(string str, Object content = null)
    {
        if (GameManager.Instance.m_IsShowLog)
        {
            Debug.LogError(str, content);
        }
    }

    /// <summary>
    /// 跑出异常
    /// </summary>
    /// <param name="e"></param>
    /// <param name="o"></param>
    public static void LogException(System.Exception e, Object o = null)
    {
        if (GameManager.Instance.m_IsShowLog)
        {
            Debug.LogException(e, o);
        }
    }

}
