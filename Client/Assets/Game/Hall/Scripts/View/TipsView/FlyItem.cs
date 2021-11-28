using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FlyItem : MonoBehaviour
{
    [SerializeField]
    private Image m_Bg;
    [SerializeField]
    private Text m_Content;



    public void Show(string content, System.Action finish) {
        gameObject.SetActive(true);
        m_Content.text = content;
        m_Bg.SetColorA(0);
        var gras = m_Bg.GetComponentsInChildren<Graphic>();
        foreach (var g in gras)
            g.SetColorA(0);

        transform.localPosition = Vector3.down * 240;
        Sequence seq = DOTween.Sequence();
        seq.Append(m_Bg.DOFade(1.0f, 0.5f));
        foreach (var g in gras)
            seq.Join(g.DOFade(1, 0.5f));
        seq.Join(transform.DOLocalMoveY(0, 0.5f));
        seq.AppendInterval(1.0f);
        seq.Append(m_Bg.DOFade(0, 0.5f));
        foreach (var g in gras)
            seq.Join(g.DOFade(0, 0.5f));
        seq.Join(transform.DOLocalMoveY(200, 0.5f));
        seq.OnComplete(()=> {
            gameObject.SetActive(false);
            finish?.Invoke();
        });
    }
}
