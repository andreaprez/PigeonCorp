using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.Bonus
{
    public class BonusModel
    {
        public int BuyButtonRateTier;
        public int EggValueMultiplierTier;
        public int EggLayingRateMultiplierTier;
        public int HatcheryCapacityIncrementTier;
        public int VehicleShippingRateIncrementTier;
        public int ResearchDiscountTier;
        public int HatcheryDiscountTier;
        public int VehicleDiscountTier;
        public int PigeonDiscountTier;
        
        public int BuyButtonRate;
        public float EggValueMultiplier;
        public float EggLayingRateMultiplier;
        public int HatcheryCapacityIncrement;
        public int VehicleShippingRateIncrement;
        public int ResearchDiscount;
        public int HatcheryDiscount;
        public int VehicleDiscount;
        public int PigeonDiscount;

        public BonusModel(BonusUserData userData, BonusTitleData config)
        {
            BuyButtonRateTier = userData.BuyButtonRateTier;
            EggValueMultiplierTier = userData.EggValueMultiplierTier;
            EggLayingRateMultiplierTier = userData.EggLayingRateMultiplierTier;
            HatcheryCapacityIncrementTier = userData.HatcheryCapacityIncrementTier;
            VehicleShippingRateIncrementTier = userData.VehicleShippingRateIncrementTier;
            ResearchDiscountTier = userData.ResearchDiscountTier;
            HatcheryDiscountTier = userData.HatcheryDiscountTier;
            VehicleDiscountTier = userData.VehicleDiscountTier;
            PigeonDiscountTier = userData.PigeonDiscountTier;

            var tiersConfig = config.BonusTiersConfiguration;
            BuyButtonRate = tiersConfig[BuyButtonRateTier].BuyButtonRate;
            EggValueMultiplier = tiersConfig[EggValueMultiplierTier].EggValueMultiplier;
            EggLayingRateMultiplier = tiersConfig[EggLayingRateMultiplierTier].EggLayingRateMultiplier;
            HatcheryCapacityIncrement = tiersConfig[HatcheryCapacityIncrementTier].HatcheryCapacityIncrement;
            VehicleShippingRateIncrement = tiersConfig[VehicleShippingRateIncrementTier].VehicleShippingRateIncrement;
            ResearchDiscount = tiersConfig[ResearchDiscountTier].ResearchDiscount;
            HatcheryDiscount = tiersConfig[HatcheryDiscountTier].HatcheryDiscount;
            VehicleDiscount = tiersConfig[VehicleDiscountTier].VehicleDiscount;
            PigeonDiscount = tiersConfig[PigeonDiscountTier].PigeonDiscount;
        }
        
        private BonusUserData Serialize()
        {
            return new BonusUserData(this);
        }
    }
}