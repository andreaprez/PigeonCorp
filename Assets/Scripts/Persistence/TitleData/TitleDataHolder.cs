using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "TitleData", menuName = "PigeonCorp/TitleData/TitleData")]
    public class TitleDataHolder : ScriptableObject
    {
        [Header("Title")]
        public MainBuyButtonTitleData MainBuyButton;
        
        [Header("User")]
        public UserStateInitialData UserState;
    }
}