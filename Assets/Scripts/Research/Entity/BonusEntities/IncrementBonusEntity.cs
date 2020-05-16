namespace PigeonCorp.Research
{
    public class IncrementBonusEntity : BonusEntity
    {
        public override void ApplyBonus()
        {
            ApplicableBonusEntity.ApplyIncrement(CurrentValue.Value);
        }
    }
}