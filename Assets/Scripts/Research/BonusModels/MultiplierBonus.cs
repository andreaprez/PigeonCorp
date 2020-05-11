using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class MultiplierBonus : BonusModel
    {
        public MultiplierBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }

        protected override void ApplyBonus()
        {
            _applicableBonusEntity.ApplyMultiplier(CurrentValue.Value);
        }
    }
}