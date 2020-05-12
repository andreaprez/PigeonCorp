using UniRx;

namespace PigeonCorp.ValueModifiers
{
    public class ResearchValueModifiers : BaseValueModifiers
    {
        public readonly ReactiveProperty<float> ResearchCostDiscount;

        public ResearchValueModifiers()
        {
            ResearchCostDiscount = new ReactiveProperty<float>(0);
        }

        public override void ApplyDiscount(float value)
        {
            ResearchCostDiscount.Value = value;
        }
    }
}