using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 版本号
    /// </summary>
    public string m_Version;
    /// <summary>
    /// 是否开启打印
    /// </summary>
    public bool m_IsShowLog = false;
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string m_ServerUrl = "";
    /// <summary>
    /// 是否用链接地址
    /// </summary>
    public bool m_UseUrl = false;
    /// <summary>
    /// 服务器IP
    /// </summary>
    public string m_Ip = "47.96.251.52";
    /// <summary>
    /// 服务器端口
    /// </summary>
    public int m_Port = 12345;


    /// <summary>
    /// 连接ID
    /// </summary>
    public int mSessionId { get; set; }

    private int mAutoReConnectNum = 0;//自动重连次数







    public int iosIsOpenID = 0;

    public bool IsTestVersion = false;


    private float mLastReconnetTime;//上一次重连的时间
    //当前设定的值为多少？
    [SerializeField]
    private int CurIsOpenId = 0;
    #region 版本数据

    public string mVersionTxtUrl = "";//检测版本地址
    public string mVersion;//游戏版本
    /// <summary>
    /// GPS工具
    /// </summary>
    public SQGPSLoader mGpsTools;
    /// <summary>
    /// 心跳连接判定断线之后的回调处理
    /// </summary>
    private CallBack mBreathCallBack;


    public static GameManager Instance { get; private set; }


    #endregion
    void Awake()
    {
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //
        Application.targetFrameRate = 30;
    }

    // Use this for initialization
    void Start()
    {
        //不可销毁对象
        GameObject.DontDestroyOnLoad(this);

        //初始化模块
        InitModule();

        //初始化游戏网络
        InitNetWork();

        //初始化延时模块
        InitDelay();

        CallBack call = () =>
        {
            //初始化创建管理器
            InitSceneMgr();

            //初始化音效管理器
            SoundProcess.Create();
#if GPS
            //初始化GPS
            InitGPS();
#endif
            //加载配置信息
            LoadConfig();

            Global.GetController<LoadingController>().Show(OnLoadFinished);
        };


        //
        if (m_UseUrl)
        {
            GetIp(call);
        }
        else
            call();
    }


    /// <summary>
    /// 解析IP地址
    /// </summary>
    /// <param name="call"></param>
    private void GetIp(CallBack call)
    {
        if (m_UseUrl)
        {
            m_Ip = SQToolHelper.DoGetHostAddresses(m_ServerUrl);
            if (string.IsNullOrEmpty(m_Ip))
            {
                //Global.Inst.GetController<CommonTipsController>().ShowTips("网络连接错误，请检查网络后重新连接", "确定", () =>
                //{
                //    GetIp(call);
                //});
            }
            else
                call();
        }
    }


    private void InitModule()
    {

        Global.RegisterController<LoadingController>();
        Global.RegisterController<LoginController>();   //登录
        Global.RegisterController<TipsController>();    //提示
        Global.RegisterController<LobbyController>();   //大厅
        Global.RegisterController<RoomSpaceController>();  //游戏场景

        //Global.Inst.RegisterController<NetLoadingController>();//网络loading
        //Global.Inst.RegisterController<MainController>();//主界面
        //Global.Inst.RegisterController<ClubController>();  //俱乐部
        //Global.Inst.RegisterController<MyTeaHouseController>();  //我的俱乐部
        //Global.Inst.RegisterController<CreateRoomController>();  //创建房间
        //Global.Inst.RegisterController<ShopController>();        //商城     
        //Global.Inst.RegisterController<RecordController>();      //战绩
        //Global.Inst.RegisterController<GameInteractionController>(); //互动表情

        //Global.Inst.RegisterController<MJGameController>();    //麻将
    }

    /// <summary>
    /// 初始化网络模块
    /// </summary>
    private void InitNetWork()
    {
        GameObject NetObject = new GameObject();
        NetObject.name = "NetProcess";
        NetObject.AddComponent<NetProcess>();
        GameObject.DontDestroyOnLoad(NetObject);

        //初始化网络
        NetProcess.InitNetWork(GameManager.Instance.m_Ip, GameManager.Instance.m_Port);
    }

    /// <summary>
    /// 初始化延时模块
    /// </summary>
    private void InitDelay()
    {
        GameObject obj = new GameObject();
        obj.name = "SQTimeOutTool";
        obj.AddComponent<SQTimeOutTool>();
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// 初始化Gps
    /// </summary>
    private void InitGPS()
    {
        GameObject GPSObject = new GameObject();
        GPSObject.name = "SQGPSLoader";

        mGpsTools = GPSObject.AddComponent<SQGPSLoader>();
        GameObject.DontDestroyOnLoad(GPSObject);
    }

    /// <summary>
    /// 初始化配置表
    /// </summary>
    private void LoadConfig()
    {
        ConfigManager.Creat();
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ConfigManager.LoadAllConfig(SQResourceLoader.LoadAssetBundle("AssetsResources/allgameconfigs"), 1, null);
        }
    }


    //一开始游戏时候，加载界面完成的回调函数
    private void OnLoadFinished()
    {
        NetProcess.Connect(m_Ip, m_Port, (b, sessionId) =>
        {
            if (b)
            {
                mSessionId = sessionId;

                Global.GetController<LoginController>().Show();
                Global.GetController<LoadingController>().CloseWindow();
            }
            else
            {
                Global.GetController<TipsController>().Show("网络错误", "连接失败", null);
            }
        });

    }


    /// <summary>
    /// 初始化场景模块
    /// </summary>
    private void InitSceneMgr()
    {
        GameObject NetObject = new GameObject();
        NetObject.name = "SQSceneLoader";
        NetObject.AddComponent<SQSceneLoader>();
        GameObject.DontDestroyOnLoad(NetObject);
    }


    /// <summary>
    /// 检测版本更新
    /// </summary>
    private void CheckVersion()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //AutoLogin();
            return;
        }

        Assets.LoadTxtFile(GameManager.Instance.mVersionTxtUrl, (t) =>
        {
            if (string.IsNullOrEmpty(t))
            {
                //Global.Inst.GetController<CommonTipsController>().ShowTips("当前网络信号差，请重新登录", "确定", true, () =>
                //{
                //    CheckVersion();
                //});
            }
            else
            {
                VersionConfig txt = JsonConvert.DeserializeObject<VersionConfig>(t);
                if (txt == null)
                {
                    //Global.Inst.GetController<CommonTipsController>().ShowTips("当前网络信号差，请重新登录", "确定", true, () =>
                    //{
                    //    CheckVersion();
                    //});
                }
                else
                {
                    string[] local = GameManager.Instance.mVersion.Split('.');
                    string[] net = new string[3];
                    string ver = "";
                    string appUrl = "";
                    string resUrl = "";
                    int resId = 0;
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        net = txt.android_version.Split('.');
                        ver = txt.android_version;
                        appUrl = txt.android_appUrl;
                        resUrl = txt.android_resUrl;
                        resId = txt.android_resVersion;
                    }
                    else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        net = txt.ios_version.Split('.');
                        ver = txt.ios_version;
                        appUrl = txt.ios_appUrl;
                        resUrl = txt.ios_resUrl;
                        resId = txt.ios_resVersion;
                    }
                    if (GameManager.Instance.mVersion != ver)
                    {
                        UpdateVersion(net, local, appUrl, resUrl, resId);
                    }
                    else
                    {
                        //ConfigManager.LoadAllConfig(resUrl, resId, AutoLogin);
                    }
                }
            }
        });
    }

    /// <summary>
    /// 判断a是否大于b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>1是相等，2是大于，-1是小于</returns>
    private void UpdateVersion(string[] a, string[] b, string appUrl, string resUrl, int resId)
    {
        Debug.Log(a);
        Debug.Log(b);
        int a1 = int.Parse(a[0]);
        int a2 = int.Parse(a[1]);
        int a3 = int.Parse(a[2]);

        int b1 = int.Parse(b[0]);
        int b2 = int.Parse(b[1]);
        int b3 = int.Parse(b[2]);
        bool isupdate = false;
        //1.0.28  1.1.0
        if (a1 > b1)
        {
            isupdate = true;
        }
        else
        {
            if (a2 > b2 && a1 == b1)
            {
                isupdate = true;
            }
            else
            {
                if (a2 == b2 && a1 == b1)
                {
                    if (a3 > b3)
                    {
                        isupdate = true;
                    }
                }

            }
        }
        if (isupdate)
        {

            //Global.Inst.GetController<CommonTipsController>().ShowTips("发现新版本，请前往下载", "下载", true, () =>
            //{
            //    if (Application.platform == RuntimePlatform.Android)
            //    {
            //        Application.OpenURL(appUrl);
            //    }
            //    else if (Application.platform == RuntimePlatform.IPhonePlayer)
            //    {
            //        Application.OpenURL(appUrl);
            //    }

            //    ConfigManager.LoadAllConfig(resUrl, resId);
            //}, () =>
            //{


            //    ConfigManager.LoadAllConfig(resUrl, resId);

            //});
        }
        else
        {
            //ConfigManager.LoadAllConfig(resUrl, resId, AutoLogin);
        }

    }


    /// <summary>
    /// 链接服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="finish"></param>
    public void ConnectServer(string ip, int port, Action finish)
    {
        //释放当前链接
        NetProcess.ReleaseConnect(mSessionId);
        NetProcess.Connect(ip, port, (b, sessionId) =>
        {
            if (b)
            {
                mSessionId = sessionId;
                finish?.Invoke();
            }
        });
    }








    void OnApplicationPause(bool b)
    {
        if (!b)//唤醒
        {
            //SQDebug.Log("程序获得焦点" + NetProcess.IsCurExistConnected());
            //if (NetProcess.IsCurExistConnected())
            //{
            //    CancelHeartBreath();
            //    StartHeartBreath();
            //}
            //else
            {
                //if (!BaseView.ContainsView<LoginView>())
                //{
                //    if (LoginModel.Inst.LoginData == null)
                //        return;
                //    ShowNetTips();
                //}
            }

        }

    }

    private void OnApplicationQuit()
    {
        NetProcess.ReleaseConnect(mSessionId);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            NetProcess.ReleaseAllConnect();
            Debug.Log("断开所有链接————————————————");
        }

        if (Input.GetKeyUp(KeyCode.A))
            OnApplicationPause(false);
    }
#endif


    #region 心跳处理

    /// <summary>
    /// 没有接受到心跳的次数
    /// </summary>
    public int mNoHeartTimes { get; private set; }

    public bool mIsStartHeart { get; private set; }

    /// <summary>
    /// 开始心跳连接
    /// </summary>
    /// <param name="call">心跳判定断线之后的回调</param>
    public void StartHeartBreath()
    {
        SQDebug.Log("开始心跳");
        mIsStartHeart = true;
        if (!IsInvoking("SendHeartBreath"))
        {
            SQDebug.Log("开启心跳连接");
            InvokeRepeating("SendHeartBreath", 0.01f, 5.0f);  //5s发送一次心跳连接
        }
    }


    /// <summary>
    /// 取消心跳连接
    /// </summary>
    public void CancelHeartBreath()
    {
        SQDebug.Log("取消心跳连接");
        CancelInvoke("SendHeartBreath");
        mNoHeartTimes = 0;
        mIsStartHeart = false;
    }


    /// <summary>
    /// 发送心跳连接
    /// </summary>
    private void SendHeartBreath()
    {
        mNoHeartTimes++;

        //超过三次没有心跳则重连
        if (mNoHeartTimes >= 3)
        {
            SQDebug.Log("通过心跳判定的断网处理");
            ShowNetTips();
            return;
        }

        HeartReq req = new HeartReq();
        NetProcess.SendRequest<HeartReq>(req, MessageProtoId.HeartReq, MessageProtoId.HeartRes, (msg) =>
        {
            var res = msg.Read<HeartRes>();
            mNoHeartTimes = 0;
            mAutoReConnectNum = 0;
        }, false);
    }


    /// <summary>
    /// 显示网络提示
    /// </summary>
    public void ShowNetTips()
    {
        if (!mIsStartHeart) return;
        //取消心跳
        CancelHeartBreath();
        //释放链接
        NetProcess.ReleaseAllConnect();

        //Global.Inst.GetController<NetLoadingController>().ShowLoading(true);

        //重连间隔要大于5秒
        if (Time.realtimeSinceStartup - mLastReconnetTime > 5)
        {
            SQDebug.Log("直接重连");
            ReConnet();
        }
        else
        {
            float time = 5 + mLastReconnetTime - Time.realtimeSinceStartup;
            SQDebug.Log("等" + time + "秒重连");
            //SetTimeout.add(time, ReConnet);
        }



    }

    /// <summary>
    /// 重连
    /// </summary>
    private void ReConnet()
    {
        mLastReconnetTime = Time.realtimeSinceStartup;//重连时间
        mSessionId = -1;
        //Global.Inst.GetController<NetLoadingController>().ShowLoading(false);

        Action call = () =>
        {
            SQDebug.Log("重连次数" + mAutoReConnectNum);
            NetProcess.Connect(m_Ip, m_Port, (isConnect, id) =>
            {
                if (isConnect)
                {
                    Global.GetController<LoginController>().Relogin();
                }
                else
                    ShowNetTips();
            });
        };
        if (mAutoReConnectNum < 3)//如果自动重连次数少于两次就弹出提示   3
        {
            mAutoReConnectNum++;
            call();
        }
        else
        {
            Global.GetController<TipsController>().Show("网络连接失败", "尝试连接|退出游戏", call, () =>
            {
                Application.Quit();
            }, null);
        }
    }
    #endregion

    [ContextMenu("ScreenShot")]
    public void ScreenShotTest()
    {
        ScreenCapture.CaptureScreenshot("screenshop.png");
    }
}


