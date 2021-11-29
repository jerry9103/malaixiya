using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSpaceView : BaseView
{
    [SerializeField]
    private UIUserInfo m_UserInfo;
    [SerializeField]
    private List<RoomSession> m_SessionList;

    public void Show(GameConfig config)
    {
        m_UserInfo.Show();

        for (int i = 0; i < config.config.Count; i++)
        {
            m_SessionList[i].Show(config.config[i], (session) =>
            {

            });
        }
    }

    public void OnBackBtnClick()
    {
        Global.GetController<LobbyController>().Show();
        Close();
    }

    public void OnAddGoldBtnClick()
    {

    }
}
