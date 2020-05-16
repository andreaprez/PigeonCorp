namespace PigeonCorp.Research.Entity
{
    public class MultiplierBonusEntity : BonusEntity
    {
        public override void ApplyBonus()
        {
            ApplicableBonusEntity.ApplyMultiplier(CurrentValue.Value);
        }
    }
}