using System;
using PigeonCorp.Bonus;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class BonusUserData
    {
        public readonly int BuyButtonRateTier;
        public readonly int EggValueMultiplierTier;
        public readonly int EggLayingRateMultiplierTier;
        public readonly int HatcheryCapacityIncrementTier;
        public readonly int VehicleShippingRateIncrementTier;
        public readonly int ResearchDiscountTier;
        public readonly int HatcheryDiscountTier;
        public readonly int VehicleDiscountTier;
        public readonly int PigeonDiscountTier;
        
        public BonusUserData()
        {
            BuyButtonRateTier = 0;
            EggValueMultiplierTier = 0;
            EggLayingRateMultiplierTier = 0;
            HatcheryCapacityIncrementTier = 0;
            VehicleShippingRateIncrementTier = 0;
            ResearchDiscountTier = 0;
            HatcheryDiscountTier = 0;
            VehicleDiscountTier = 0;
            PigeonDiscountTier = 0;
        }
        
        public BonusUserData(BonusModel model)
        {
            BuyButtonRateTier = model.BuyButtonRateTier;
            EggValueMultiplierTier = model.EggValueMultiplierTier;
            EggLayingRateMultiplierTier = model.EggLayingRateMultiplierTier;
            HatcheryCapacityIncrementTier = model.HatcheryCapacityIncrementTier;
            VehicleShippingRateIncrementTier = model.VehicleShippingRateIncrementTier;
            ResearchDiscountTier = model.ResearchDiscountTier;
            HatcheryDiscountTier = model.HatcheryDiscountTier;
            VehicleDiscountTier = model.VehicleDiscountTier;
            PigeonDiscountTier = model.PigeonDiscountTier;
        }
    }
}