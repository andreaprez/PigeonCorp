using UniRx;

namespace PigeonCorp.ValueModifiers.Entity
{
    public class ShippingValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> VehicleShippingRateIncrement;
        public readonly ReactiveProperty<float> VehicleCostDiscount;

        public ShippingValueModifiers()
        {
            VehicleShippingRateIncrement = new ReactiveProperty<float>(0);
            VehicleCostDiscount = new ReactiveProperty<float>(0);
        }

        public override void ApplyIncrement(float value)
        {
            VehicleShippingRateIncrement.Value = value;
        }

        public override void ApplyDiscount(float value)
        {
            VehicleCostDiscount.Value = value;
        }
    }
}