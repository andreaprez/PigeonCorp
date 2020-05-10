using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Research
{
    public class BonusModel
    {
        public readonly BonusType Type;
        public readonly ReactiveProperty<int> Tier;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> CurrentValue;
        public readonly ReactiveProperty<float> NextValue;
        public readonly ReactiveProperty<float> NextCost;

        private readonly BonusConfig _config;
        
        public BonusModel(BonusConfig config, BonusState stateData)
        {
            _config = config;

            Type = config.Type;
            Name = new ReactiveProperty<string>(config.Name);
            Icon = new ReactiveProperty<Sprite>(config.Icon);
            Tier = new ReactiveProperty<int>(stateData.CurrentTier);
            
            CurrentValue = new ReactiveProperty<float>();
            NextValue = new ReactiveProperty<float>();
            NextCost = new ReactiveProperty<float>();

            SetProperties();
        }

        private void SetProperties()
        {
            CurrentValue.Value = _config.Tiers[Tier.Value].Value;
            
            if (Tier.Value < _config.Tiers.Count - 1)
            {
                NextValue.Value = _config.Tiers[Tier.Value + 1].Value;
                NextCost.Value = _config.Tiers[Tier.Value + 1].Cost;
            }
        }
        
        public void Research()
        {
            Tier.Value += 1;
            SetProperties();
        }
    }
}