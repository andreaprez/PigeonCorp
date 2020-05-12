using PigeonCorp.Research;
using UniRx;

namespace PigeonCorp.ValueModifiers
{
    public class MainBuyButtonValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> PigeonsPerClickMultiplier;

        public MainBuyButtonValueModifiers()
        {
            PigeonsPerClickMultiplier = new ReactiveProperty<float>(1);
        }
        
        public override void ApplyMultiplier(float value)
        {
            PigeonsPerClickMultiplier.Value = value;
        }
    }
}