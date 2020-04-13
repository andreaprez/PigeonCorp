using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.UserState;
using UniRx;

namespace PigeonCorp.Hatchery
{
    public class HatcheriesModel
    {
        public readonly List<HatcheryModel> Hatcheries;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<int> UsedCapacity;
        public readonly ReactiveProperty<int> TotalProduction;

        private readonly HatcheriesTitleData _config;

        public HatcheriesModel(
            HatcheriesTitleData config,
            HatcheriesUserData userData,
            UserStateModel userStateModel
        )
        {
            _config = config;
            
            Hatcheries = new List<HatcheryModel>();
            InitHatcheries(userData.Hatcheries);
            
            MaxCapacity = new ReactiveProperty<int>(CalculateMaxCapacity());
            UsedCapacity = new ReactiveProperty<int> (userStateModel.CurrentPigeons.Value);
            
            TotalProduction = new ReactiveProperty<int>(CalculateTotalProduction());
        }

        public void UpdateUsedCapacity(int currentPigeons)
        {
            UsedCapacity.Value = currentPigeons;
        }

        public void UpdateMaxCapacity()
        {
            MaxCapacity.Value = CalculateMaxCapacity();
        }
        
        public void UpdateTotalProduction()
        {
            TotalProduction.Value = CalculateTotalProduction();
        }

        private void InitHatcheries(List<HatcheryState> hatcheriesData)
        {
            for (int i = 0; i < hatcheriesData.Count; i++)
            {
                var hatchery = new HatcheryModel(_config, hatcheriesData[i]);
                Hatcheries.Add(hatchery);
            }
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

        private int CalculateTotalProduction()
        {
            var production = 0;
            
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
        
        public HatcheriesUserData Serialize()
        {
            return new HatcheriesUserData(this);
        }
    }
}