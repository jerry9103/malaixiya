using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserInfo : MonoBehaviour
{
    [SerializeField]
    private Image m_Head;
    [SerializeField]
    private Image m_Sex;
    [SerializeField]
    private Text m_Nick;
    [SerializeField]
    private Text m_Id;
    [SerializeField]
    private Text m_Gold;
    [SerializeField]
    private Sprite[] m_SexSp;


    public void Show()
    {
        Assets.LoadLocalIcon(UserInfoData.headUrl, m_Head);
        if (m_Sex != null)
            m_Sex.sprite = UserInfoData.Sex == 1 ? m_SexSp[1] : m_SexSp[0];
        m_Nick.text = UserInfoData.NickName;
        m_Id.text = string.Format("ID:{0}", UserInfoData.UserId);
        m_Gold.text = UserInfoData.Gold.ToString();
    }
}
