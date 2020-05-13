using PigeonCorp.Research;

namespace PigeonCorp.ValueModifiers
{
    public abstract class BaseValueModifiers : IApplicableBonus
    {
        public virtual void ApplyMultiplier(float value) { }
 
        public virtual void ApplyIncrement(float value) { }
 
        public virtual void ApplyDiscount(float value) { }
    }
}