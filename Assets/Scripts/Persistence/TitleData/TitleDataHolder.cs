using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "TitleData", menuName = "PigeonCorp/TitleData/TitleData")]
    public class TitleDataHolder : ScriptableObject
    {
        public MainBuyButtonTitleData MainBuyButton;
    }
}