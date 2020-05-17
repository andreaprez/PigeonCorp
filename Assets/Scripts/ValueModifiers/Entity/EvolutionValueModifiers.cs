using UniRx;

namespace PigeonCorp.ValueModifiers.Entity
{
    public class EvolutionValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> EggValueMultiplier;

        public EvolutionValueModifiers()
        {
            EggValueMultiplier = new ReactiveProperty<float>(1);
        }

        public override void ApplyMultiplier(float value)
        {
            EggValueMultiplier.Value = value;
        }
    }
}