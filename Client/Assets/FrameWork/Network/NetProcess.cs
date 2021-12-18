using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine;
using System.Text;
using System;

public enum NETSTATE
{
    CLOSENET,
    NETREALEASE
}



public class NetProcess : MonoBehaviour
{

    public static NetProcess It;

    public delegate void MessegeCallBack(MessageData msgData);

    public delegate void ConnectCallBack(bool connectResult, int sessionID);
    public delegate void ConnectStateCallBack(NETSTATE state, int sessionId);

    public static List<string> LockResponseId = new List<string>();
    //游戏内部协议 
    public static List<string> mGameTypeList = new List<string>();
    //大厅协议
    public static List<string> mHallTypeList = new List<string>();

    private static ConnectionManager mConnectionManager = null;
    private readonly static Dictionary<string, MessegeCallBack> ResonponseCallDic = new Dictionary<string, MessegeCallBack>();
    private static ConnectCallBack connCallBack = null;
    private static Connection GameCenter = null;
    public static ConnectStateCallBack NetConnectStateCall;
    public static int mCurConnectSessionId = -1;
    //
    private static string mIpAdress;
    private static int mPort;

    void Awake()
    {
        It = this;
        mConnectionManager = new ConnectionManager();
        mConnectionManager.OnConnectEvent = OnExecConnect;
        mConnectionManager.OnReceiveEvent = OnExecReceive;
        mConnectionManager.OnCloseEvent = OnExecClose;
    }

    private void Start()
    {

    }

    public static void InitNetWork(string Ip, int port)
    {
        mIpAdress = Ip;
        mPort = port;
    }

    public static void Connect(string Ip, int port, ConnectCallBack callBack)
    {
        Connection c = mConnectionManager.CreateConnection(Ip, port);
        connCallBack = callBack;
    }

    public static void ReleaseConnect(int sessionID = -1)
    {
        if (mConnectionManager.CurrentConnection == null)
            return;
        if (sessionID == -1)
        {
            mConnectionManager.ReleaseConnection(mConnectionManager.CurrentConnection);
        }
        else
        {
            mConnectionManager.ReleaseConnection(sessionID);
        }


    }
    /// <summary>
    /// 清除所有连接
    /// </summary>
    public static void ReleaseAllConnect()
    {
        mConnectionManager.ReleaseAllConnection();
        ConnectionManager.Instance.MessageEventQueue.ClearAll();
    }
    public static void BackToGameCenter(int sessionId)
    {
        mConnectionManager.BackToGameCenter(sessionId);
    }

    /// <summary>
    /// Registers the response call back.
    /// </summary>
    /// <param name="msgType">Message type.</param>
    /// <param name="msgCallBack">Message call back.</param>
    /// <param name="gameType">Game type.  不为0的则为游戏内协议 </param>
    public static void RegisterResponseCallBack(string msgName, MessegeCallBack msgCallBack, int gameType = 0)
    {
        ResonponseCallDic[msgName] = msgCallBack;

        if (gameType == 0)
        {
            mHallTypeList.Add(msgName);
        }
        else
        {
            mGameTypeList.Add(msgName);
        }

    }

    public static void UnRegisterResponseCallBack(string msgType)
    {
        if (ResonponseCallDic.ContainsKey(msgType))
        {
            ResonponseCallDic.Remove(msgType);
        }
    }

    public static void CleanResponseProtoByType(int gameType)
    {
        if (gameType == 0)
        {
            for (int i = 0; i < mHallTypeList.Count; i++)
            {
                UnRegisterResponseCallBack(mHallTypeList[i]);
            }
        }
        else
        {

            for (int i = 0; i < mGameTypeList.Count; i++)
            {
                UnRegisterResponseCallBack(mGameTypeList[i]);
            }

        }

    }

    #region XLUA


    public delegate void SendRequestCall(object data);

    /// <summary>
    /// Lua调用方法 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cmdId"></param>
    /// <param name="call"></param>

    public static void SendMsgByLua(byte[] data, string cmdId, SendRequestCall call, bool isShowLock = true)
    {
        SendRequestLuaMsg(data, cmdId, (msg) =>
        {
            SQDebug.Log("wor2222iyi=" + msg.msgData.Length);
            if (call != null)
            {
                call(msg.msgData);
            }
        }, isShowLock);
    }


    /// <summary>
    /// Lua调用注册方法
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="call"></param>

    public static void RegisterMsgByLua(string msgType, SendRequestCall call)
    {
        RegisterResponseCallBack(msgType, (msg) =>
        {
            if (call != null)
            {
                call(msg.msgData);
            }
        });
    }

    #endregion



    public static void SendRequest<T>(object o, string sendName, string recivName, MessegeCallBack msgCallBack = null, bool showlock = true)
    {
        if (showlock)
        {

        }
        Action act = () =>
        {
            MessageData msg = new MessageData();
            msg.Write<T>(o, sendName);
            ResonponseCallDic[recivName] = msgCallBack;
            if (msgCallBack != null)
            {
                SetLockScreen(recivName, showlock);
            }
            ConnectionManager.Instance.CurrentConnection.SendMessage(msg.msgData);
        };

        if (mConnectionManager.CurrentConnection.Connected)
        {
            act();
        }
        else
        {
            //Connect(false, (result, sessionId) =>
            //{
            //    if (result)
            //    {
            //        act();
            //    }
            //});
        }
    }

    /// <summary>
    /// 提供给LUA过来调用的接口
    /// </summary>
    /// <param name="o"></param>
    /// <param name="msgType"></param>
    /// <param name="msgCallBack"></param>
    /// <param name="showlock"></param>
	public static void SendRequestLuaMsg(byte[] o, string msgType, MessegeCallBack msgCallBack = null, bool showlock = true)
    {
        Action act = () =>
        {
            MessageData msg = new MessageData();
            msg.WriteLua(o, msgType);
            ResonponseCallDic[msgType] = msgCallBack;
            if (msgCallBack != null)
            {
                SetLockScreen(msgType, showlock);
            }
            ConnectionManager.Instance.CurrentConnection.SendMessage(msg.msgData);
        };

        if (mConnectionManager.CurrentConnection.Connected)
        {
            act();
        }
        else
        {
            Connect(GameManager.Instance.m_Ip, GameManager.Instance.m_Port, (result, sessionId) =>
            {
                if (result)
                {
                    act();
                }
            });
        }
    }


    public static void SetLockScreen(string unLockResponseID, bool isShow)
    {
        if (!LockResponseId.Contains(unLockResponseID))
        {
            if (LockResponseId.Count == 0 && isShow)
            {
                //Global.It.mCommonBoxCtr.ShowLoading(true);
            }
            LockResponseId.Add(unLockResponseID);
        }
    }

    void Update()
    {
        if (mConnectionManager != null)
        {
            mConnectionManager.Update();
        }
    }


    /// <summary>
    /// 判断是否还有链接存在
    /// </summary>
    /// <returns></returns>
    public static bool IsCurExistConnected()
    {
        if (mConnectionManager == null)
            return true;
        if (mConnectionManager != null && mConnectionManager.CurrentConnection != null)
            return mConnectionManager.CurrentConnection.Connected;
        return false;
    }


    void OnExecConnect(Connection connection, SOCKET_ERRCODE errCode)
    {
        if (errCode != SOCKET_ERRCODE.SUCCESS)
        {
            SQDebug.Log("failed to connect to server:" + connection.SessionID.ToString() + "; socket:" + connection.ToString() + " ; error code : " + errCode.ToString());
            connCallBack(false, 0);
            return;
        }
        SQDebug.Log("success to connect server; socket:" + connection.SessionID.ToString());
        ConnectionManager.Instance.CurrentConnection = connection;
        connCallBack(true, connection.SessionID);
    }

    bool OnExecReceive(Connection connection, string key, byte[] msgData)
    {
        if (LockResponseId.Contains(key))
        {
            LockResponseId.Remove(key);
            if (LockResponseId.Count == 0)
            {
                //CommonUI.Instance.HideLockScreen();
            }
        }
        //if (key != 1920)

        if (msgData == null)
        {
            return false;
        }

        MessageData msg = new MessageData(msgData);

        string jsonString = Encoding.UTF8.GetString(msgData, 0, msgData.Length);
        //if (key != 1102)
        //{
        //    SQDebug.PrintToScreen("Json=" + jsonString);
        //    SQDebug.Log("Json=" + jsonString);
        //}

        if (!ResonponseCallDic.ContainsKey(key))
        {
            SQDebug.Log("Client Don`t wanna handle this messege ,msgType : " + key);
            return false;
        }
        MessegeCallBack callBack = ResonponseCallDic[key];
        if (callBack == null)
        {
            SQDebug.Log("callBack Is Null ,msgType : " + key);
            return false;
        }
        else
        {
            //Global.It.mCommonBoxCtr.ShowLoading(false);
            //CommonUI.It.HideLoad();
        }

        if (!callBack.Method.IsStatic && callBack.Target.ToString() == "null")
        {
            SQDebug.LogWarning("对象已经被销毁了，而消息还想被处理");

            if (NetConnectStateCall != null)
            {
                //连接已释放
                NetConnectStateCall(NETSTATE.NETREALEASE, connection.SessionID);
            }
            return false;

        }

        callBack(msg);
        return true;
    }


    void OnExecClose(Connection connection, SOCKET_ERRCODE errCode)
    {
        SQDebug.Log("Close Socket! socket sessionid :" + connection.SessionID + "; errCode:" + connection.ExceptionCode + "; errMsg:" + connection.ExceptionMessage);
        ConnectionManager.Instance.ReleaseConnection(connection);
        ConnectionManager.Instance.MessageEventQueue.ClearAll();

        GameManager.Instance.ShowNetTips();
        if (NetConnectStateCall != null)
        {
            NetConnectStateCall(NETSTATE.CLOSENET, connection.SessionID);
        }

    }

}


