
public class LoginModel : BaseModel
{
    public static LoginModel Inst;

    public override void SetController(BaseController c)
    {
        base.SetController(c);
        Inst = this;
    }


    public static PlayerBaseInfo Data { get; private set; }

    public static string UserId { get; private set; }

    public void SetLoginData(PlayerBaseInfo data, string userId) {
        UserId = userId;
        Data = data;
    }
}
