using UnityEngine;

namespace GameGoing.SDK
{
	public class Example_ChangeLanguage : MonoBehaviour 
	{
		public void SetLanguage_English()
		{
			SetLanguage("English");
		}

		public void SetLanguage_French()
		{
			SetLanguage("French");
		}

		public void SetLanguage_Spanish()
		{
			SetLanguage("Spanish");
		}


		public void SetLanguage( string LangName )
		{
			if( LocalizationManager.HasLanguage(LangName))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}

	}
}