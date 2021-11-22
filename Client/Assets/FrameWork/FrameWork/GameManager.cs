using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using System.Text;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 是否开启打印
    /// </summary>
    public bool mIsShowLog = false;
    public string ServerUrl = "";
    public bool mUseUrl = false;
    public string Ip = "47.96.251.52";
    public int port = 12345;
    public static GameManager Instance;
    public int iosIsOpenID = 0;
    public bool mIsFirstLogin;   //是否登陆
    public bool IsTestVersion = false;

    private int mAutoReConnectNum = 0;//自动重连次数
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

    #endregion
    void Awake()
    {
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //
        Application.targetFrameRate = 30;
        mIsFirstLogin = false;
    }

    // Use this for initialization
    void Start()
    {
        //不可销毁对象
        GameObject.DontDestroyOnLoad(this);
        //gameObject.AddComponent<SetTimeout>();
        //初始化模块
        InitModule();
        //游戏网络
        InitNetWork();

        //
        InitDelay();

        CallBack call = () =>
        {
            NetProcess.InitNetWork(GameManager.Instance.Ip, GameManager.Instance.port);

            //初始化创建管理器
            InitSceneMgr();

            //初始化音效管理器
            SoundProcess.Create();


            //初始化SDK
            InitSixSdkManager();
#if YYVOICE
            //开启YY语音
            if (Application.platform != RuntimePlatform.WindowsEditor)
                YYsdkManager.Create();
#endif
#if GPS
            //初始化GPS
            InitGPS();
#endif
            //加载配置信息
            LoadConfig();
        };

        //
        if (mUseUrl)
        {
            GetIp(call);
        }
        else
            call();
    }

    //一开始游戏时候，加载界面完成的回调函数
    public void OnLoadFinished()
    {
        //Global.It.mLoginCtrl.OpenWindow();
        //Global.It.mLoadingCtrl.mloadUI.HideWindow();
    }

    private void GetIp(CallBack call)
    {
        if (mUseUrl)
        {
            Ip = SQToolHelper.DoGetHostAddresses(ServerUrl);
            if (string.IsNullOrEmpty(Ip))
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



    /// <summary>
    /// 初始化网络
    /// </summary>
    private void InitNetWork()
    {
        GameObject NetObject = new GameObject();
        NetObject.name = "NetProcess";
        NetObject.AddComponent<NetProcess>();
        NetObject.AddComponent<HttpProcess>();
        GameObject.DontDestroyOnLoad(NetObject);
    }
    /// <summary>
    /// 初始化Gps
    /// </summary>
    private void InitGPS()
    {
        GameObject GPSObject = new GameObject();
        GPSObject.name = "SQGPSLoader";
        
        mGpsTools= GPSObject.AddComponent<SQGPSLoader>();
        GameObject.DontDestroyOnLoad(GPSObject);
    }

    private void InitSceneMgr()
    {
        GameObject NetObject = new GameObject();
        NetObject.name = "SQSceneLoader";
        NetObject.AddComponent<SQSceneLoader>();
        GameObject.DontDestroyOnLoad(NetObject);
    }

    private void InitDelay()
    {
        GameObject obj = new GameObject();
        obj.name = "SQTimeOutTool";
        obj.AddComponent<SQTimeOutTool>();
        GameObject.DontDestroyOnLoad(obj);
    }


    #region 初始化模块
    private void InitModule()
    {
        //Global.Inst.RegisterController<LoadingController>();//加载
        //Global.Inst.RegisterController<LoginController>();//登录
        //Global.Inst.RegisterController<CommonTipsController>();//公共提示框
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
    #endregion
    /// <summary>
    /// 后台设置的值大于版本的值，true :微信登陆，false : 游客登陆
    /// </summary>
    /// <returns></returns>
    public bool IsGuestOpen()
    {
        return this.iosIsOpenID > this.CurIsOpenId;
    }

    private void LoadConfig()
    {
        ConfigManager.Creat();
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ConfigManager.LoadAllConfig(SQResourceLoader.LoadAssetBundle("AssetsResources/allgameconfigs"), 1, null);
        }
    }

    /// <summary>
    /// 初始化SDK
    /// </summary>
    private void InitSixSdkManager()
    {
        GameObject NetObject = new GameObject();
        NetObject.name = "SixqinSDKManager";
        //NetObject.AddComponent<SixqinSDKManager>();
        GameObject.DontDestroyOnLoad(NetObject);
    }

    void OnApplicationPause(bool b)
    {
        if (!b)//唤醒
        {
            SQDebug.Log("程序获得焦点"+ NetProcess.IsCurExistConnected());
            if (NetProcess.IsCurExistConnected())
            {
                CancelHeartBreath();
                StartHeartBreath();
            }
            else
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

    public int mNoHeartTime = 0;//没有接收到心跳的次数

    /// <summary>
    /// 开始心跳连接
    /// </summary>
    /// <param name="call">心跳判定断线之后的回调</param>
    public void StartHeartBreath()
    {
        SQDebug.Log("开始心跳");
        if (!IsInvoking("SendHeartBreath"))
        {
            SQDebug.Log("开启心跳连接");
            InvokeRepeating("SendHeartBreath", 0.01f, 5.0f);//5s发送一次心跳连接
        }
    }


    /// <summary>
    /// 取消心跳连接
    /// </summary>
    public void CancelHeartBreath()
    {
        CancelInvoke("SendHeartBreath");
        SQDebug.Log("取消心跳连接");
        mNoHeartTime = 0;
    }


    /// <summary>
    /// 发送心跳连接
    /// </summary>
    public void SendHeartBreath()
    {
        mNoHeartTime++;
        if (mNoHeartTime >= 3)//3
        {//超过两次没有收到心跳恢复
            SQDebug.Log("通过心跳判定的断网处理");
            ShowNetTips();
            return;
        }

        //NetProcess.SendRequest<int>(0, ProtoIdMap.CMD_HearBreath, (msg) =>
        //{
        //    mNoHeartTime = 0;
        //    mAutoReConnectNum = 0;
        //    //SQDebug.Log("心跳返回+++++++++++++++++++++++++++++++++++++");
        //}, false);
    }


    /// <summary>
    /// 显示网络提示
    /// </summary>
    public void ShowNetTips()
    {
        GameManager.Instance.CancelHeartBreath();
        NetProcess.ReleaseAllConnect();
        //Global.Inst.GetController<NetLoadingController>().ShowLoading(true);
        //SetTimeout.remove(ReConnet);
        if (Time.realtimeSinceStartup - mLastReconnetTime > 5)//重连间隔要大于5秒
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
        //mLastReconnetTime = Time.realtimeSinceStartup;//重连时间
        //LoginModel.Inst.mSessionId = -1;
        //Global.Inst.GetController<NetLoadingController>().ShowLoading(false);
        //CallBack call = () =>
        //{
        //    SQDebug.Log("重连次数" + mAutoReConnectNum);
        //    NetProcess.Connect(GameManager.Instance.Ip, GameManager.Instance.port, (isConnect, id) =>
        //    {
        //        if (isConnect)
        //        {
        //            StartHeartBreath();
        //            Global.Inst.GetController<LoginController>().Relogin();
        //        }
        //        else
        //            ShowNetTips();
        //    });
        //};
        //if (mAutoReConnectNum < 3)//如果自动重连次数少于两次就弹出提示   3
        //{
        //    mAutoReConnectNum++;
        //    call();
        //}
        //else
        //{
        //    Global.Inst.GetController<CommonTipsController>().ShowTips("网络连接失败", "尝试连接|退出游戏", true, call, () =>
        //    {
        //        Application.Quit();
        //    }, null, "网络异常");
        //}
    }
    #endregion

    [ContextMenu("ScreenShot")]
    public void ScreenShotTest()
    {
        ScreenCapture.CaptureScreenshot("screenshop.png");
    }
}


