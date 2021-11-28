

public class LoginSendData {
    public string account;
    public string password;
    public int ver;
    public int type;
    public string assetkey;
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