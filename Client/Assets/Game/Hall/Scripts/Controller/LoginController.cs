
public class LoginController : BaseController
{
    LoginView mView;

    public LoginController() : base(typeof(LoginView).Name, "UILogin/UILoginMain") {
        SetModel<LoginModel>();
    }


    public override BaseView OpenWindow()
    {
         mView =  base.OpenWindow<LoginView>();
        return mView;
    }


    public void Show() {
        OpenWindow();
    }


    private void LoginGateServer(LoginReq req, LoginRes res)
    {
        EnterGameReq gameReq = new EnterGameReq();
        gameReq.version = req.version;
        gameReq.device_info = "1";
        gameReq.uId = res.uId;
        gameReq.rId = res.rId;
        gameReq.expireTime = res.expireTime;
        gameReq.loginToken = res.loginToken;

        NetProcess.SendRequest<EnterGameReq>(gameReq, MessageProtoId.EnterGameReq, MessageProtoId.EnterGameRes, (data) =>
        {
            EnterGameRes gameRes = data.Read<EnterGameRes>();
            if (gameRes.errCode == 0)
            {
                SQDebug.Log("登录成功");

                //开始心跳
                GameManager.Instance.StartHeartBreath();

                LoginModel.Inst.SetLoginData(gameRes.baseInfo, res.uId.ToString());
                Global.GetController<LobbyController>().Show();
                CloseWindow();

                GameManager.Instance.StartHeartBreath();
            }
            else
            {
                SQDebug.Log("登录Gate失败:" + gameRes.errMsg);
            }
        });
    }


    public void SendLogin(LoginReq req) {
        NetProcess.SendRequest<LoginReq>(req, MessageProtoId.LoginReq, MessageProtoId.LoginRes, (data) =>
        {
            LoginRes res = data.Read<LoginRes>();
            if (res.errCode == 0)
            {
                //登录gate服务器
                var serverInfo = res.gateSvrs[0].gameGates;
                serverInfo = serverInfo.Replace("\"", "").Remove(0, 1);
                serverInfo = serverInfo.Remove(serverInfo.Length - 1, 1);
                var servers = serverInfo.Split(':');
                GameManager.Instance.ConnectServer(servers[1], int.Parse(servers[2]), () =>
                {
                    LoginGateServer(req, res);
                });
            }
            else
            {
                SQDebug.Log("登录认证失败:" + res.errMsg);
            }
        });
    }

    public void Relogin() { 
        
    }
}
