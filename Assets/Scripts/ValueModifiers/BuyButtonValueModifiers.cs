using PigeonCorp.Research;
using UniRx;

namespace PigeonCorp.ValueModifiers
{
    public class BuyButtonValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> PigeonsPerClickMultiplier;

        public BuyButtonValueModifiers()
        {
            PigeonsPerClickMultiplier = new ReactiveProperty<float>(1);
        }
        
        public override void ApplyMultiplier(float value)
        {
            PigeonsPerClickMultiplier.Value = value;
        }
    }
}