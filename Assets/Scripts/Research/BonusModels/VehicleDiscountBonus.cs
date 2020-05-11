using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Research
{
    public class VehicleDiscountBonus : BonusModel
    {
        public VehicleDiscountBonus(BonusConfig config, BonusState stateData) : base(config, stateData)
        {
        }
        
        protected override void ApplyBonus()
        {
            // TODO
        }
    }
}