
using System.Collections.Generic;

public class RoomDeskData 
{
    public string roomId;
    public int state;
    public int playerCount;
    public List<RoomDeskPlayer> players;
}


public class RoomDeskPlayer {
    public string head;
    public string nick;
}


public class SendJoinRoomData {
    public int uid;
    public int roomid;
    public int group;
    public string unionid;
}
