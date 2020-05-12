using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class MultiplierBonus : BonusModel
    {
        public MultiplierBonus(
            BonusConfig config,
            BonusState stateData,
            IApplicableBonus applicableBonus
        ) : base(config, stateData, applicableBonus)
        {
        }
        
        public override void ApplyBonus()
        {
            _applicableBonusEntity.ApplyMultiplier(CurrentValue.Value);
        }
    }
}