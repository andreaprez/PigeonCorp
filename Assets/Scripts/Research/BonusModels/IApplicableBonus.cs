namespace PigeonCorp.Research
{
    public interface IApplicableBonus
    {
        void ApplyMultiplier(float value);
        void ApplyIncrement(float value);
        void ApplyDiscount(float value);
    }
}