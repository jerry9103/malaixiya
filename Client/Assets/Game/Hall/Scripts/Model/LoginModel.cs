
public class LoginModel : BaseModel
{
    public static LoginModel Inst;

    public override void SetController(BaseController c)
    {
        base.SetController(c);
        Inst = this;
    }


    public static LoginBackData Data { get; private set; }

    public void SetLoginData(LoginBackData data) {
        Data = data;
    }
}
