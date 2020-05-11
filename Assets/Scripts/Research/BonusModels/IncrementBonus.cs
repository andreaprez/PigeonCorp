using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class IncrementBonus : BonusModel
    {
        public IncrementBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }
        
        protected override void ApplyBonus()
        {
            _applicableBonusEntity.ApplyIncrement(CurrentValue.Value);
        }
    }
}