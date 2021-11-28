using System;

public class TipsController : BaseController
{
    TipsView mView;
    public TipsController() : base(typeof(TipsView).Name, "UITip/UITip") { }


    public override BaseView OpenWindow()
    {
        mView = base.OpenWindow<TipsView>();
        return mView;
    }


    public void Show(string title, string content, Action sure, Action cancel = null, Action close = null) {
        OpenWindow();
        mView.Show(title, content, sure, cancel, close);
    }

    public void Show(string content) {
        OpenWindow();
        mView.Show(content);
    }
}
