using System;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Hatcheries
{
    public class HatcheryModel
    {
        public readonly ReactiveProperty<bool> Built;
        public readonly ReactiveProperty<int> Level;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> NextCost;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<int> UsedCapacity;
        public readonly ReactiveProperty<float> EggLayingRate;
        
        private readonly HatcheriesTitleData _config;

        public HatcheryModel(HatcheriesTitleData config, HatcheryState stateData)
        {
            _config = config;

            Built = new ReactiveProperty<bool>(stateData.Built);
            Level = new ReactiveProperty<int>(stateData.Level);

            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            NextCost = new ReactiveProperty<float>(0);
            MaxCapacity = new ReactiveProperty<int>(0);
            UsedCapacity = new ReactiveProperty<int>(0);
            EggLayingRate = new ReactiveProperty<float>(0);
            
            if (Built.Value)
            {
                SetProperties();
            }
            else
            {
                NextCost.Value = config.HatcheriesConfiguration[Level.Value].Cost;
            }
        }

        public void Build()
        {
            Built.Value = true;
            Level.Value = 1;

            SetProperties();
        }
        
        public void Upgrade()
        {
            Level.Value += 1;
            SetProperties();
        }

        public void SetUsedCapacity(int quantity)
        {
            UsedCapacity.Value = quantity;
        }

        public void ApplyMultiplierToEggLayingRate(float multiplier)
        {
            if (Built.Value)
            { 
                var baseValue = _config.HatcheriesConfiguration[Level.Value - 1].EggLayingRate;
                EggLayingRate.Value = baseValue * multiplier;
            }
        }
        
        public void ApplyIncrementToMaxCapacity(float increment)
        {
            if (Built.Value)
            {
                var baseValue = _config.HatcheriesConfiguration[Level.Value - 1].MaxCapacity;
                var incrementValue = MathUtils.CalculateQuantityFromPercentage(
                    increment,
                    MaxCapacity.Value
                );
                MaxCapacity.Value = baseValue + (int)incrementValue;
            }
        }
        
        private void SetProperties()
        {
            Name.Value = _config.HatcheriesConfiguration[Level.Value - 1].Name;
            Icon.Value = _config.HatcheriesConfiguration[Level.Value - 1].Icon;
            MaxCapacity.Value = _config.HatcheriesConfiguration[Level.Value - 1].MaxCapacity;
            EggLayingRate.Value = _config.HatcheriesConfiguration[Level.Value - 1].EggLayingRate;
            if (Level.Value < _config.HatcheriesConfiguration.Count)
            {
                NextCost.Value = _config.HatcheriesConfiguration[Level.Value].Cost;
            }
        }
    }
}