using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListWidget : BaseViewWidget
{
    [SerializeField]
    private RoomDeskItem m_Item;
    [SerializeField]
    private Transform m_DeskRoot;

    private List<RoomDeskData> list = new List<RoomDeskData>();

    public void Show(GameConfigData config) {
        for (int i = 0; i < 20; i++) {
            RoomDeskData data = new RoomDeskData();
            data.roomId = "612487";
            data.playerCount = 0;

            list.Add(data);
        }

        RoomDeskItem item;
        for (int i = 0; i < list.Count; i++) {
            if (m_DeskRoot.childCount > i)
            {
                item = m_DeskRoot.GetChild(i).GetComponent<RoomDeskItem>();
            }
            else {
                item = Instantiate<RoomDeskItem>(m_Item);
                item.transform.SetParent(m_DeskRoot);
                item.transform.localEulerAngles = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;
            }

            item.Show(list[i], (data)=> {
                Global.GetController<RoomSpaceController>().SendJoinRoom(data.roomId);
            });
        }
    }
}
