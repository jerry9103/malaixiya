

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