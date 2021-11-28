using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class LoadingView : BaseView
{
    [SerializeField]
    private Image m_Slider;
    [SerializeField]
    private Text m_SliderTxt;
    [SerializeField]
    private Text m_Version;


    public void Show(System.Action finish)
    {
        m_Version.text = "V" + Application.version;

        DOTween.To(() => 0, (x) =>
        {
            m_Slider.fillAmount = x;
            m_SliderTxt.text = string.Format("{0}%", Mathf.RoundToInt(x * 100));
        }, 1.0f, 1.0f).OnComplete(() =>
        {
            finish?.Invoke();
        });
    }
}
