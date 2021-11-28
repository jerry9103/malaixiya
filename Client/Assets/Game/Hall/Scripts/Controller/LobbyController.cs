using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : BaseController
{
    LobbyView mView;
    public LobbyController() : base(typeof(LobbyView).Name, "UILobby/UILobby") { }



    public override BaseView OpenWindow()
    {
        mView = base.OpenWindow<LobbyView>();
        return mView;
    }


    public void Show() {
        OpenWindow();
        mView.Show();
    }

}
