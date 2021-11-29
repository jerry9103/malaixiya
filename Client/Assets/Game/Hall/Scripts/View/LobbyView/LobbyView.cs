using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyView : BaseView
{
    [SerializeField]
    private UIUserInfo m_UserInfo;

    public void Show()
    {
        m_UserInfo.Show();
    }


    public void OnGameBtnClick(GameObject go)
    {
        int id = int.Parse(go.name.Split('_')[1]);

        List<ConfigDada> configs = ConfigManager.GetConfigs<GameConfig>();
        foreach (var con in configs)
        {
            var config = con as GameConfig;
            if (config.gameCode.Equals(id))
            {
                if (config.isOpen)
                    Global.GetController<RoomSpaceController>().Show(config);
                else
                    Global.GetController<TipsController>().Show("未开放");
            }
        }
    }
}
