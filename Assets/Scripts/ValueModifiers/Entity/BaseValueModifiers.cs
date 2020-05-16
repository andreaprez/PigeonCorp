using PigeonCorp.Research.Entity;

namespace PigeonCorp.ValueModifiers.Entity
{
    public abstract class BaseValueModifiers : IApplicableBonus
    {
        public virtual void ApplyMultiplier(float value) { }
 
        public virtual void ApplyIncrement(float value) { }
 
        public virtual void ApplyDiscount(float value) { }
    }
}