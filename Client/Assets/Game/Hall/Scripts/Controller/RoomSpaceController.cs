using Newtonsoft.Json;
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

    public void Show(GameConfig config)
    {
        OpenWindow();
        mView.Show(config);
    }


    public void SendJoinRoom(string roomId)
    {
        var data = new SendJoinRoomData();
        data.roomid = int.Parse(roomId);
        data.uid = int.Parse(UserInfo.UserId);
        data.unionid = UserInfo.OpenId;
        data.group = -1;

        var url = GameManager.Instance.m_ServerUrl.GetPostUrl("loginmsg", "join", data);

        HttpProcess.SendPost(url, (code, msg) =>
        {

        });
    }
}
