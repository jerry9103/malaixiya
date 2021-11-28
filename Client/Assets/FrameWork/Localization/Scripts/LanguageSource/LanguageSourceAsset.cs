using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGoing.SDK
{
    [CreateAssetMenu(fileName = "I2Languages", menuName = "GameGoing Localization/LanguageSource", order = 1)]
    public class LanguageSourceAsset : ScriptableObject, ILanguageSource
    {
        public  LanguageSourceData SourceData
        {
            get { return mSource; }
            set { mSource = value; }
        }

        public LanguageSourceData mSource = new LanguageSourceData();
    }
}