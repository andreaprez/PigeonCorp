using System;
using System.Collections.Generic;
using PigeonCorp.UserState;
using UniRx;

namespace Hatchery
{
    public class HatcheriesModel
    {
        public readonly List<HatcheryModel> Hatcheries;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<int> UsedCapacity;

        public HatcheriesModel(UserStateModel userStateModel)
        {
            Hatcheries = new List<HatcheryModel>();
            for (int i = 0; i < 4; i++)
            {
                Hatcheries.Add(new HatcheryModel());
            }
            
            MaxCapacity = new ReactiveProperty<int>(CalculateMaxCapacity());
            UsedCapacity = new ReactiveProperty<int> (userStateModel.CurrentPigeons.Value);
        }

        public void UpdateUsedCapacity(int current)
        {
            UsedCapacity.Value = current;
        }
        
        private int CalculateMaxCapacity()
        {
            var maxCapacity = 0;
            
            foreach (var hatchery in Hatcheries)
            {
                if (hatchery.Built)
                {
                    maxCapacity += hatchery.MaxCapacity;
                }
            }

            return maxCapacity;
        }
    }
}