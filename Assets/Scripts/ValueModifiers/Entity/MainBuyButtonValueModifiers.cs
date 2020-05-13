using UniRx;

namespace PigeonCorp.ValueModifiers
{
    public class MainBuyButtonValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> PigeonsPerClickMultiplier;
        public readonly ReactiveProperty<float> PigeonCostDiscount;

        public MainBuyButtonValueModifiers()
        {
            PigeonsPerClickMultiplier = new ReactiveProperty<float>(1);
            PigeonCostDiscount = new ReactiveProperty<float>(0);
        }
        
        public override void ApplyMultiplier(float value)
        {
            PigeonsPerClickMultiplier.Value = value;
        }

        public override void ApplyDiscount(float value)
        {
            PigeonCostDiscount.Value = value;
        }
    }
}