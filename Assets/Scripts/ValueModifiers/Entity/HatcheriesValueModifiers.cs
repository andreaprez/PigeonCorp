using UniRx;

namespace PigeonCorp.ValueModifiers.Entity
{
    public class HatcheriesValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> EggLayingRateMultiplier;
        public readonly ReactiveProperty<float> HatcheryCapacityIncrement;
        public readonly ReactiveProperty<float> HatcheryCostDiscount;

        public HatcheriesValueModifiers()
        {
            EggLayingRateMultiplier = new ReactiveProperty<float>(1);
            HatcheryCapacityIncrement = new ReactiveProperty<float>(0);
            HatcheryCostDiscount = new ReactiveProperty<float>(0);
        }
        
        public override void ApplyMultiplier(float value)
        {
            EggLayingRateMultiplier.Value = value;
        }

        public override void ApplyIncrement(float value)
        {
            HatcheryCapacityIncrement.Value = value;
        }

        public override void ApplyDiscount(float value)
        {
            HatcheryCostDiscount.Value = value;
        }
    }
}