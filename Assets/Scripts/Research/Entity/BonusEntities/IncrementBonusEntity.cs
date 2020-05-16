namespace PigeonCorp.Research.Entity
{
    public class IncrementBonusEntity : BonusEntity
    {
        public override void ApplyBonus()
        {
            ApplicableBonusEntity.ApplyIncrement(CurrentValue.Value);
        }
    }
}