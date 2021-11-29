using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSession : MonoBehaviour
{
    [SerializeField]
    private Text m_SessionName;
    [SerializeField]
    private Text m_BaseScore;
    [SerializeField]
    private Text m_MinScore;

    public void Show(GameConfigData data, System.Action<RoomSession> click) {
        m_SessionName.text = data.levelName;
        m_BaseScore.text = "底分:" + data.baseScore.ToString();
        m_MinScore.text = "入场资格:" + data.minScore.ToString();

        GetComponent<Button>().onClick.AddListener(()=> {
            click?.Invoke(this);
        });
    }
}
