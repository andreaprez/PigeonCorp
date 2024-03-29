using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "TitleData", menuName = "PigeonCorp/GameConfig/TitleData")]
    public class TitleDataHolder : ScriptableObject
    {
        public UserStateTitleData UserState;
        public PigeonTitleData Pigeon;
        public HatcheriesTitleData Hatcheries;
        public ShippingTitleData Shipping;
        public ResearchTitleData Research;
        public EvolutionTitleData Evolution;
    }
}