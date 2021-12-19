

[ProtoBuf.ProtoContract]
public class ClientMsgHead {
    [ProtoBuf.ProtoMember(1)]
    public int msgtype;
    [ProtoBuf.ProtoMember(2)]
    public string msgname;
    [ProtoBuf.ProtoMember(3)]
    public string svr_id;
    [ProtoBuf.ProtoMember(4)]
    public int service_address;
}


[ProtoBuf.ProtoContract]
public class Version
{
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