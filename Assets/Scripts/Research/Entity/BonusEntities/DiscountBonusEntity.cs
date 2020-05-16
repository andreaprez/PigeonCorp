namespace PigeonCorp.Research
{
    public class DiscountBonusEntity : BonusEntity
    {
        public override void ApplyBonus()
        {
            ApplicableBonusEntity.ApplyDiscount(CurrentValue.Value);
        }
    }
}