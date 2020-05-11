using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class PigeonDiscountBonus : BonusModel
    {
        public PigeonDiscountBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }
        
        protected override void ApplyBonus()
        {
            // TODO
        }
    }
}