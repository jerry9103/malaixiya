using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyView : BaseView
{
    [SerializeField]
    private UIUserInfo m_UserInfo;

    public void Show() {
        m_UserInfo.Show();
    }
}
