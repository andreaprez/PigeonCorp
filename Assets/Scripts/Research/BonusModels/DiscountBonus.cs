using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class DiscountBonus : BonusModel
    {
        public DiscountBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }
        
        protected override void ApplyBonus()
        {
            _applicableBonusEntity.ApplyDiscount(CurrentValue.Value);
        }
    }
}