using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClickSound : MonoBehaviour
{
    private void Awake()
    {
        foreach (var btn in GetComponentsInChildren<Button>(true)) {
            btn.onClick.AddListener(()=> {
                SoundProcess.PlaySound("click");
            });
        }
    }
}
