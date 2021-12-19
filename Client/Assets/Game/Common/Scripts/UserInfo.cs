
public static class UserInfoData
{
    public static int Ver { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    public static string headUrl { get { return "HeadImage/head" + LoginModel.Data.logo; } }

    /// <summary>
    /// 昵称
    /// </summary>
    public static string NickName { get { return LoginModel.Data.roleName; } }
    /// <summary>
    /// 性别
    /// </summary>
    public static int Sex { get { return LoginModel.Data.sex; } }
    /// <summary>
    /// 用户ID
    /// </summary>
    public static string UserId { get { return LoginModel.UserId; } }

    /// <summary>
    /// 金币
    /// </summary>
    public static float Gold { get { return LoginModel.Data.coin; } }
}
