using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class HatcheryDiscountBonus : BonusModel
    {
        public HatcheryDiscountBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }
        
        protected override void ApplyBonus()
        {
            // TODO
        }
    }
}