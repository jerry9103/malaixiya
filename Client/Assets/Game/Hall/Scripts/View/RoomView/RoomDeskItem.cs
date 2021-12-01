using UnityEngine;
using UnityEngine.UI;

public class RoomDeskItem : MonoBehaviour
{
    [SerializeField]
    private Text m_RoomId;
    [SerializeField]
    private Text m_RoomState;
    [SerializeField]
    private Button m_JoinBtn;

    private RoomDeskData mData;

    public void Show(RoomDeskData data, System.Action<RoomDeskData> click) {
        mData = data;

        m_JoinBtn.onClick.AddListener(()=> {
            click?.Invoke(data);
        });
    }
}
