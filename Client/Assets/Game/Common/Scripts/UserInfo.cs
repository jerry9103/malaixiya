
public static class UserInfo
{
    public static int Ver { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    public static string headUrl { get { return "HeadImage/head" + LoginModel.Data.imgurl; } }

    /// <summary>
    /// 昵称
    /// </summary>
    public static string NickName { get { return LoginModel.Data.name; } }
    /// <summary>
    /// 性别
    /// </summary>
    public static int Sex { get { return LoginModel.Data.sex; } }
    /// <summary>
    /// 用户ID
    /// </summary>
    public static string UserId { get { return LoginModel.Data.uid; } }

    /// <summary>
    /// 金币
    /// </summary>
    public static float Gold { get { return LoginModel.Data.gold; } }
    /// <summary>
    /// 用户openid
    /// </summary>
    public static string OpenId { get { return LoginModel.Data.openid; } }
}
