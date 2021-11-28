
public class LoginController : BaseController
{
    LoginView mView;

    public LoginController() : base(typeof(LoginView).Name, "UILogin/UILoginMain") {
        SetModel<LoginModel>();
    }


    public override BaseView OpenWindow()
    {
         mView =  base.OpenWindow<LoginView>();
        return mView;
    }


    public void Show() {
        OpenWindow();
    }
}
