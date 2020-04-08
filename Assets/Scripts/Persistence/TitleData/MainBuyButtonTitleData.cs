using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "MainBuyButtonTitleData", menuName = "PigeonCorp/TitleData/MainBuyButton")]
    public class MainBuyButtonTitleData : ScriptableObject
    {
        public int PigeonsPerClick;
    }
}