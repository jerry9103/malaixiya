using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameGoing.SDK
{
    [AddComponentMenu("GameGoing/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
        public UnityEvent _OnLocalize = new UnityEvent();
		
		public void Enable()
		{
            LocalizationManager.OnLocalizeEvent -= OnLocalize;
            LocalizationManager.OnLocalizeEvent += OnLocalize;
        }

        public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= OnLocalize;
		}

		public void OnLocalize()
		{
            _OnLocalize.Invoke();
        }
   }
}