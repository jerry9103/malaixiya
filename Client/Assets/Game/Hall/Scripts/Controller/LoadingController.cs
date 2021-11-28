using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : BaseController
{
    LoadingView mView;
    public LoadingController() : base(typeof(LoadingView).Name, "UILoading/UILoading")
    {

    }


    public override BaseView OpenWindow()
    {
        mView = base.OpenWindow<LoadingView>();
        return mView;
    }

    public void Show(System.Action finish)
    {
        OpenWindow();
        mView.Show(finish);
    }
}
