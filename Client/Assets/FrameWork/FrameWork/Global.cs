using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class Global : MonoBehaviour
{
    /// <summary>
    /// UI节点
    /// </summary>
    public Transform m_UIRoot;

    public static Global Inst;
    //当前网络ID
    private int mCurSessionId = 0;

    private Dictionary<string, BaseController> ControllerDic = new Dictionary<string, BaseController>();
    #region controller

    #endregion

    void Awake()
    {
        Inst = this;
        GameObject.DontDestroyOnLoad(this);
    }

    public static void RegisterController<T>() where T : new()
    {
        T t = new T();
        string name = typeof(T).Name;
        if (!Inst.ControllerDic.ContainsKey(name))
            Inst.ControllerDic.Add(name, t as BaseController);
    }

    public static T GetController<T>() where T : BaseController
    {
        string name = typeof(T).Name;
        BaseController bc = null;
        if (Inst.ControllerDic.TryGetValue(name, out bc))
            return bc as T;
        return null;

    }


    /// <summary>
    /// 连接游戏服务器的
    /// </summary>
    /// <param name="call">Call.</param>
	public void ConnectServer(Action<bool> call)
    {
        NetProcess.Connect(GameManager.Instance.m_Ip, GameManager.Instance.m_Port, (isok, sessionId) =>
        {

            if (isok)
            {
                mCurSessionId = sessionId;
            }
            //
            if (call != null)
            {
                call(isok);
            }

        });


    }
}

