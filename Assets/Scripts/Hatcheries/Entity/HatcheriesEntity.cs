using System.Collections.Generic;
using PigeonCorp.MainTopBar;
using PigeonCorp.Utils;
using UniRx;
using Random = UnityEngine.Random;

namespace PigeonCorp.Hatcheries
{
    public class HatcheriesEntity
    {
        public MainTopBarEntity MainTopBarEntity;
        
        public readonly List<HatcheryEntity> Hatcheries;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<int> UsedCapacity;
        public readonly ReactiveProperty<float> TotalProduction;

        public HatcheriesEntity()
        {
            Hatcheries = new List<HatcheryEntity>();
            MaxCapacity = new ReactiveProperty<int>();
            UsedCapacity = new ReactiveProperty<int>();
            TotalProduction = new ReactiveProperty<float>();
        }
        
        public void UpdateUsedCapacity()
        {
            UsedCapacity.Value = CalculateUsedCapacity();
        }

        public void UpdateMaxCapacity()
        {
            MaxCapacity.Value = CalculateMaxCapacity();
        }
        
        public void UpdateTotalProduction()
        {
            TotalProduction.Value = CalculateTotalProduction();
        }

        public void UpdateUsedCapacityOfAllHatcheries()
        {
            foreach (var hatchery in Hatcheries)
            {
                var percentageOfTotalCapacity = MathUtils.CalculatePercentageDecimalFromQuantity(
                    hatchery.MaxCapacity.Value,
                    MaxCapacity.Value
                );
                var usedQuantityFromPercentage =
                    percentageOfTotalCapacity * UsedCapacity.Value;

                hatchery.UsedCapacity.Value = (int)usedQuantityFromPercentage;
            }
        }
        
        public int GetRandomBuiltHatcheryId()
        {
            var randomId = Random.Range(0, Hatcheries.Count);
            while (!Hatcheries[randomId].Built.Value)
            {
                randomId = Random.Range(0, Hatcheries.Count);
            }
            return randomId;
        }
        
        private int CalculateMaxCapacity()
        {
            var maxCapacity = 0;
            foreach (var hatchery in Hatcheries)
            {
                if (hatchery.Built.Value)
                {
                    maxCapacity += hatchery.MaxCapacity.Value;
                }
            }
            return maxCapacity;
        }

        private int CalculateUsedCapacity()
        {
            var currentPigeons = MainTopBarEntity.PigeonsCount.Value;
            
            if (currentPigeons < MaxCapacity.Value)
            {
                return MainTopBarEntity.PigeonsCount.Value;
            }
            return MaxCapacity.Value;
        }

        private float CalculateTotalProduction()
        {
            var production = 0f;
            foreach (var hatchery in Hatcheries)
            {
                if (hatchery.Built.Value)
                {
                    var hatcheryPigeons = hatchery.UsedCapacity.Value;
                    var hatcheryLayingRate = hatchery.EggLayingRate.Value;
                    var hatcheryProduction = hatcheryPigeons * hatcheryLayingRate;
                    production += hatcheryProduction;
                }
            }
            return production;
        }
    }
}