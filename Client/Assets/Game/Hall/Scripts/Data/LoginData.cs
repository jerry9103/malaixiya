
[ProtoBuf.ProtoContract]
public class Version {
    /// <summary>
    /// 平台id
    /// </summary>
    [ProtoBuf.ProtoMember(1)]
    public int platform;
    /// <summary>
    /// 渠道id
    /// </summary>
    [ProtoBuf.ProtoMember(2)]
    public int channel;
    /// <summary>
    /// 版本号
    /// </summary>
    [ProtoBuf.ProtoMember(3)]
    public string version;
    /// <summary>
    /// 账号类型
    /// </summary>
    [ProtoBuf.ProtoMember(4)]
    public int authtype;
    /// <summary>
    /// 描述从哪里注册过来的 
    /// </summary>
    [ProtoBuf.ProtoMember(5)]
    public int regfrom;
}


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
    public int errcode;
    [ProtoBuf.ProtoMember(2)]
    public string errcodedes;
}


public class LoginBackData {
    public string uid;
    public string name;
    public string imgurl;
    public float gold;
    public float savegold;
    public int card;
    public string ip;
    public int sex;
    public int room;
    public string center;
    public string openid;
    public string passwd;
    public bool ver;
    public int gameover;
    public string key;
    public GameModel gamemode;
}

public class GameModel {
    public int[] goldgame;
    public int[] roomgame;
    public int moneymode;
    public string assetkey;
    public object config;
}