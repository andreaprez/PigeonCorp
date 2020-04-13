using System;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "BonusTitleData", menuName = "PigeonCorp/GameConfig/Bonus")]
    public class BonusTitleData : ScriptableObject
    {
        public List<BonusTier> BonusTiersConfiguration;
    }

    [Serializable]
    public class BonusTier
    {
        public int BuyButtonRate;
        public float EggValueMultiplier;
        public float EggLayingRateMultiplier;
        public int HatcheryCapacityIncrement;
        public int VehicleShippingRateIncrement;
        public int ResearchDiscount;
        public int HatcheryDiscount;
        public int VehicleDiscount;
        public int PigeonDiscount;
    }
}