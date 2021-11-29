using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpaceController : BaseController
{
    RoomSpaceView mView;
    public RoomSpaceController() : base(typeof(RoomSpaceView).Name, "UIRoom/UIRoomSpace") { }


    public override BaseView OpenWindow()
    {
        mView = base.OpenWindow<RoomSpaceView>();
        return mView;
    }

    public void Show(GameConfig config) {
        OpenWindow();
        mView.Show(config);
    }
}
