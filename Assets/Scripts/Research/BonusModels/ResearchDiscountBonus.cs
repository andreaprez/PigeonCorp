using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class ResearchDiscountBonus : BonusModel
    {
        public ResearchDiscountBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }
        
        protected override void ApplyBonus()
        {
            // TODO
        }
    }
}