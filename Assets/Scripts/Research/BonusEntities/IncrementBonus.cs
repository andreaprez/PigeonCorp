using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class IncrementBonus : BonusModel
    {
        public IncrementBonus(
            BonusConfig config,
            BonusState stateData,
            IApplicableBonus applicableBonus
        ) : base(config, stateData, applicableBonus)
        {
        }
        
        public override void ApplyBonus()
        {
            _applicableBonusEntity.ApplyIncrement(CurrentValue.Value);
        }
    }
}