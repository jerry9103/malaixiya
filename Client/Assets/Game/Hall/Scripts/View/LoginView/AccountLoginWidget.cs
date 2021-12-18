using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;

public class AccountLoginWidget : BaseViewWidget
{
    [SerializeField]
    private InputField m_Account;
    [SerializeField]
    private InputField m_PassWord;

    /// <summary>
    /// 登录
    /// </summary>
    public void OnLoginBtnClick()
    {
        //if (string.IsNullOrEmpty(m_Account.text)) {
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
        req.version.platform = 1;
        req.version.version = Application.version;
        req.version.channel = 1;
        req.version.regfrom = 1;

        NetProcess.SendRequest<LoginReq>(req, "LoginReq", "LoginRes", (data)=> {
            LoginRes res = data.Read<LoginRes>();

            SQDebug.Log("登录数据:" + JsonConvert.SerializeObject(res));
        });
    }

    public void OnCloseBtnClick() {
        Close();
    }

    public void OnForgetPassBtnClick() { }

}
