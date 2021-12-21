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
        req.uid = 1000002;
        req.uidtype = 1;
        req.deviceinfo = "pc";
        req.username = "jerry";
        req.version = new Version();
        req.version.authtype = 1;
        req.version.platform = 100;
        req.version.version = Application.version;
        req.version.channel = 1000;
        req.version.regfrom = 1;

        Global.GetController<LoginController>().SendLogin(req);
    }

    public void OnCloseBtnClick()
    {
        Close();
    }

    public void OnForgetPassBtnClick()
    {

    }

}
