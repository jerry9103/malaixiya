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

        LoginSendData data = new LoginSendData();
        data.account = "sot_abc2"; // m_Account.text;
        data.password = "123123"; // m_PassWord.text;
        data.ver = 1024;
        data.assetkey = "ea6b2efbdd4255a9f1b3bbc6399b58f4";
        data.type = 0;

        HttpProcess.SendPost(GameManager.Instance.m_ServerUrl.GetPostUrl("loginmsg", "loginPwd", data), (code, msg) =>
        {
            Debug.Log("code=" + code + " Msg=" + msg);

            HttpMessageData backData = JsonConvert.DeserializeObject<HttpMessageData>(msg);
            if (backData != null)
            {
                var loginData = JsonConvert.DeserializeObject<LoginBackData>(backData.data);
                SQDebug.Log(loginData);

                LoginModel.Inst.SetLoginData(loginData);

                Global.GetController<LobbyController>().Show();
                Global.GetController<LoginController>().CloseWindow();
            }
        });
    }

    public void OnCloseBtnClick() { }

    public void OnForgetPassBtnClick() { }

}
