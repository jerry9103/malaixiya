
public static class UserInfo
{
    /// <summary>
    /// 头像地址
    /// </summary>
    public static string headUrl { get { return LoginModel.Data.imgurl; } }

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
}
