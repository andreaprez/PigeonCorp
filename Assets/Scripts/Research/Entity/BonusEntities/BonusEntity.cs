using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Research
{
    public abstract class BonusEntity
    {
        public int Id;
        public BonusType Type;
        public ValueUnitType UnitType;
        public IApplicableBonus ApplicableBonusEntity;

        public readonly ReactiveProperty<int> Tier;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> CurrentValue;
        public readonly ReactiveProperty<float> NextValue;
        public readonly ReactiveProperty<float> NextCost;

        public BonusEntity()
        {
            Tier = new ReactiveProperty<int>();
            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            CurrentValue = new ReactiveProperty<float>();
            NextValue = new ReactiveProperty<float>();
            NextCost = new ReactiveProperty<float>();
        }

        public void Research()
        {
            Tier.Value += 1;
        }

        public virtual void ApplyBonus() { }
    }
}