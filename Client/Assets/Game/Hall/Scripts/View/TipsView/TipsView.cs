using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class TipsView : BaseView
{
    [SerializeField]
    private GameObject m_SelectObj;
    [SerializeField]
    private Button m_SureBtn;
    [SerializeField]
    private Button m_CloseBtn;
    [SerializeField]
    private Button m_CancelBtn;
    [SerializeField]
    private Text m_SelectTitle;
    [SerializeField]
    private Text m_SelectConten;
    [SerializeField]
    private Transform m_FlyParent;
    [SerializeField]
    private FlyItem m_FlyItem;

    private Queue<FlyItem> flyQue = new Queue<FlyItem>();


    public void Show(string title, string content, Action sure, Action cancel, Action close)
    {
        Action finish = () =>
        {
            m_SelectObj.SetActive(false);
            Hide();
        };

        m_SelectObj.SetActive(true);
        m_CancelBtn.gameObject.SetActive(cancel != null);
        m_CloseBtn.gameObject.SetActive(close != null);

        m_CancelBtn.onClick.AddListener(() =>
        {
            cancel?.Invoke();
        });

        m_CloseBtn.onClick.AddListener(() =>
        {
            close?.Invoke();
        });

        m_SureBtn.onClick.AddListener(() =>
        {
            sure?.Invoke();
        });

        m_SelectTitle.text = title;
        m_SelectConten.text = content;
    }

    public void Show(string content)
    {
        FlyItem item;
        if (flyQue.Count > 0)
        {
            item = flyQue.Dequeue();
        }
        else
        {
            item = Instantiate<FlyItem>(m_FlyItem);
            item.transform.SetParent(m_FlyParent);
            item.transform.localScale = Vector3.one;
            item.transform.eulerAngles = Vector3.zero;
        }
        item.transform.SetParent(m_FlyParent);
        item.Show(content, () =>
        {
            flyQue.Enqueue(item);
        });
    }
}
