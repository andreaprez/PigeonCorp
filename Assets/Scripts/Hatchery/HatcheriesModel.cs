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
        }

        public void UpdateUsedCapacity(int current)
        {
            UsedCapacity.Value = current;
        }

        public void UpdateMaxCapacity()
        {
            MaxCapacity.Value = CalculateMaxCapacity();
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
        
        // TODO: Update user data when hatcheries change
        public HatcheriesUserData Serialize()
        {
            return new HatcheriesUserData(this);
        }
    }
}