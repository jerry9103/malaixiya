
using System.Collections.Generic;


[ProtoBuf.ProtoContract]
public class LoginReq {
    [ProtoBuf.ProtoMember(1)]
    public Version version;
    [ProtoBuf.ProtoMember(2)]
    public string deviceinfo;
    [ProtoBuf.ProtoMember(3)]
    public int uid;
    [ProtoBuf.ProtoMember(4)]
    public int uidtype;
    [ProtoBuf.ProtoMember(5)]
    public string username;
}


[ProtoBuf.ProtoContract]
public class LoginRes {
    [ProtoBuf.ProtoMember(1)]
    public int errCode;
    [ProtoBuf.ProtoMember(2)]
    public string errMsg;
    [ProtoBuf.ProtoMember(3)]
    public int uId;
    [ProtoBuf.ProtoMember(4)]
    public int rId;
    [ProtoBuf.ProtoMember(5)]
    public string loginToken;   //登录服务器返回的登录token
    [ProtoBuf.ProtoMember(6)]
    public int expireTime;    //过期时间
    [ProtoBuf.ProtoMember(7)]
    public List<GateSvrItem> gateSvrs; //gate服务器地址列表
    [ProtoBuf.ProtoMember(8)]
    public string refreshToken;  //微信刷新token
    
}

[ProtoBuf.ProtoContract]
public class GateSvrItem {
    [ProtoBuf.ProtoMember(1)]
    public string ip;
    [ProtoBuf.ProtoMember(2)]
    public int port;
    [ProtoBuf.ProtoMember(3)]
    public int updateTime;
    [ProtoBuf.ProtoMember(4)]
    public int onlineEnum;
    [ProtoBuf.ProtoMember(5)]
    public string gameGates;
}


[ProtoBuf.ProtoContract]
public class EnterGameReq {
    [ProtoBuf.ProtoMember(1)]
    public Version version;
    [ProtoBuf.ProtoMember(2)]
    public string device_info; // 设备信息
    [ProtoBuf.ProtoMember(3)]
    public int uId;
    [ProtoBuf.ProtoMember(4)]
    public int rId;
    [ProtoBuf.ProtoMember(5)]
    public int expireTime;
    [ProtoBuf.ProtoMember(6)]
    public string loginToken;
    [ProtoBuf.ProtoMember(7)]
    public UserInfo userInfo;
}

[ProtoBuf.ProtoContract]
public class UserInfo {
    [ProtoBuf.ProtoMember(1)]
    public string nickName;
    [ProtoBuf.ProtoMember(2)]
    public string openId;
    [ProtoBuf.ProtoMember(3)]
    public int sex;
    [ProtoBuf.ProtoMember(4)]
    public string icon;
    [ProtoBuf.ProtoMember(5)]
    public string icon_m;
    [ProtoBuf.ProtoMember(6)]
    public string icom_l;
    [ProtoBuf.ProtoMember(7)]
    public string country;
    [ProtoBuf.ProtoMember(8)]
    public string province;
    [ProtoBuf.ProtoMember(9)]
    public string city;
}


[ProtoBuf.ProtoContract]
public class EnterGameRes{
    [ProtoBuf.ProtoMember(1)]
    public int errCode;
    [ProtoBuf.ProtoMember(2)]
    public string errMsg;
    [ProtoBuf.ProtoMember(3)]
    public int isReauth;   //是否需要重新认证，断线重连时根据token是否过期告诉客户端是否需要重新认证
    [ProtoBuf.ProtoMember(4)]
    public int serverTime;   //同步服务器时间
    [ProtoBuf.ProtoMember(5)]
    public PlayerBaseInfo baseInfo;  //判断玩家是否需要牌桌短线重连
    [ProtoBuf.ProtoMember(6)]
    public string ip;    //gate
    [ProtoBuf.ProtoMember(7)]
    public int port;   
    [ProtoBuf.ProtoMember(8)]
    public int game_id;    //游戏类型
    [ProtoBuf.ProtoMember(9)]
    public string roomsrv_id;  //房间服务器id
    [ProtoBuf.ProtoMember(10)]
    public int roomsvr_table_address;  //桌子的服务器地址
    [ProtoBuf.ProtoMember(11)]
    public int id;   // 桌子id
    [ProtoBuf.ProtoMember(12)]
    public int isNew;  //是否新手 0 否 1 是
    [ProtoBuf.ProtoMember(13)]
    public List<FriendList> frendList;   //好友列表
}

[ProtoBuf.ProtoContract]
public class PlayerBaseInfo {
    [ProtoBuf.ProtoMember(1)]
    public int rId;
    [ProtoBuf.ProtoMember(2)]
    public string roleName;   //昵称
    [ProtoBuf.ProtoMember(3)]
    public string logo;       //logo
    [ProtoBuf.ProtoMember(4)]
    public string phone;      //手机号
    [ProtoBuf.ProtoMember(5)]
    public int diamond;     //钻石
    [ProtoBuf.ProtoMember(6)]
    public long coin;       //金币
    [ProtoBuf.ProtoMember(7)]
    public long maxCoin;    //历史最大金币
    [ProtoBuf.ProtoMember(8)]
    public int sex;         //性别
    [ProtoBuf.ProtoMember(9)]
    public string desc;     //签名
    [ProtoBuf.ProtoMember(10)]
    public GamePlayInfo mjPlayInfo;  //麻将打牌信息
    [ProtoBuf.ProtoMember(11)]
    public int vip;         //vip
    [ProtoBuf.ProtoMember(12)]
    public int today_win;   //今日盈利
    [ProtoBuf.ProtoMember(13)]
    public int total_recharge;  //累计充值
    [ProtoBuf.ProtoMember(14)]
    public long boxCoin;    //保险箱金币
    [ProtoBuf.ProtoMember(15)]
    public int vipExps;      //vip经验
}


[ProtoBuf.ProtoContract]
public class FriendList {
    [ProtoBuf.ProtoMember(1)]
    public int rId;      //朋友id
    [ProtoBuf.ProtoMember(2)]
    public int isNewChat;  // 是否有新的消息 2 有信息
}


[ProtoBuf.ProtoContract]
public class GamePlayInfo {
    [ProtoBuf.ProtoMember(1)]
    public int offlineNum;  //掉线次数
    [ProtoBuf.ProtoMember(2)]
    public int winNum;  //胜局
    [ProtoBuf.ProtoMember(3)]
    public int loseNum;   //败局
    [ProtoBuf.ProtoMember(4)]
    public int highContinueWin;  //最大连胜
    [ProtoBuf.ProtoMember(5)]
    public int highMultiple;    //最高番数
    [ProtoBuf.ProtoMember(6)]
    public int maxGetCoin;   //最大获取金币
    [ProtoBuf.ProtoMember(7)]
    public int highCards;    //最大牌型
    [ProtoBuf.ProtoMember(8)]
    public List<MjHuCardTypes> mjhu_zhongleis; //胡牌种类
    [ProtoBuf.ProtoMember(9)]
    public List<MjDiCards> mjdi_cards;  //麻将底牌
}

[ProtoBuf.ProtoContract]
public class MjHuCardTypes {
    [ProtoBuf.ProtoMember(1)]
    public int cardType;   //类型
    [ProtoBuf.ProtoMember(2)]
    public int num;   //次数
}

[ProtoBuf.ProtoContract]
public class MjXLHuCardTypes {
    [ProtoBuf.ProtoMember(1)]
    public int huCardType;  //类型
    [ProtoBuf.ProtoMember(2)]
    public List<MjHuCardTypes> huZhongleis;
}

[ProtoBuf.ProtoContract]
public class MjDiCards {
    [ProtoBuf.ProtoMember(1)]
    public int type_val;   //类型 碰 1 暗杠2 直杠3 弯杠
    [ProtoBuf.ProtoMember(2)]
    public List<int> cards;  //具体牌型
}


/// <summary>
/// 通知在其他设备上登录
/// </summary>
[ProtoBuf.ProtoContract]
public class RepeatNtc {
    [ProtoBuf.ProtoMember(1)]
    public int rId;
}
