using UnityEngine.UI;
using UnityEngine;

public class LoginView : BaseView
{
    [SerializeField]
    private Toggle m_UserAgree;


    /// <summary>
    /// 检测用户协议
    /// </summary>
    /// <returns></returns>
    private bool CheckUserAgreement() {
        if (m_UserAgree.isOn)
        {

        }
        else
        {

        }

        return m_UserAgree.isOn;
    }

    /// <summary>
    /// 手机登录
    /// </summary>
    public void OnMobileBtnClick() {

    }

    /// <summary>
    /// 账号登录
    /// </summary>
    public void OnAccountBtnClick() {
        if (CheckUserAgreement()) {
            var wid = GetWidget<AccountLoginWidget>("UILogin/UIAccountLogin", transform);
        }
    }

    /// <summary>
    /// 注册账号
    /// </summary>
    public void OnRegisterBtnClick() { 
        
    }
    /// <summary>
    /// 通知消息
    /// </summary>
    public void OnNoticeBtnClick() { }

    /// <summary>
    /// 服务
    /// </summary>
    public void OnServiceBtnClick() { 
    
    }
}
