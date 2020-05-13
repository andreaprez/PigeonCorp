using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class DiscountBonus : BonusModel
    {
        public DiscountBonus(
            BonusConfig config,
            BonusState stateData,
            IApplicableBonus applicableBonus
        ) : base(config, stateData, applicableBonus)
        {
        }
        
        public override void ApplyBonus()
        {
            _applicableBonusEntity.ApplyDiscount(CurrentValue.Value);
        }
    }
}