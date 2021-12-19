using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;



public class AccountLoginWidget : BaseViewWidget
{
    [SerializeField]
    private InputField m_Account;
    [SerializeField]
    private InputField m_PassWord;

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
                LoginModel.Inst.SetLoginData(gameRes.baseInfo, res.uId.ToString());
                Global.GetController<LobbyController>().Show();
                Global.GetController<LoginController>().CloseWindow();
            }
            else
            {
                SQDebug.Log("登录Gate失败:" + gameRes.errMsg);
            }
        });
    }


    /// <summary>
    /// 登录
    /// </summary>
    public void OnLoginBtnClick()
    {
        //if (string.IsNullOrEmpty(m_Account.text))
        //{
        //    Global.GetController<TipsController>().Show("账号为空");
        //    return;
        //}
        //if (string.IsNullOrEmpty(m_PassWord.text))
        //{
        //    Global.GetController<TipsController>().Show("密码为空");
        //    return;
        //}


        LoginReq req = new LoginReq();
        req.uid = 1000001;
        req.uidtype = 1;
        req.deviceinfo = "pc";
        req.username = "jack";
        req.version = new Version();
        req.version.authtype = 1;
        req.version.platform = 100;
        req.version.version = Application.version;
        req.version.channel = 1000;
        req.version.regfrom = 1;

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

    public void OnCloseBtnClick()
    {
        Close();
    }

    public void OnForgetPassBtnClick()
    {

    }

}
