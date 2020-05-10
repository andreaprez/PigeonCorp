using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.UserState;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Hatcheries
{
    public class HatcheriesModel
    {
        public readonly List<HatcheryModel> Hatcheries;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<int> UsedCapacity;
        public readonly ReactiveProperty<int> TotalProduction;
        
        private readonly HatcheriesTitleData _config;
        private readonly UserStateModel _userStateModel;
        private List<Transform> _hatcheryEntrances;

        public HatcheriesModel(
            HatcheriesTitleData config,
            HatcheriesUserData userData,
            UserStateModel userStateModel
        )
        {
            _config = config;
            _userStateModel = userStateModel;

            Hatcheries = new List<HatcheryModel>();
            InitHatcheries(userData.Hatcheries);
            
            MaxCapacity = new ReactiveProperty<int>(CalculateMaxCapacity());
            UsedCapacity = new ReactiveProperty<int> (CalculateUsedCapacity());
            
            TotalProduction = new ReactiveProperty<int>(CalculateTotalProduction());
        }

        public void SetHatcheryEntrances(List<Transform> entrances)
        {
            _hatcheryEntrances = entrances;
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
        
        public Transform GetRandomBuiltHatchery()
        {
            var randomId = Random.Range(0, Hatcheries.Count);
            while (!Hatcheries[randomId].Built.Value)
            {
                randomId = Random.Range(0, Hatcheries.Count);
            }

            return _hatcheryEntrances[randomId];
        }
       
        public HatcheriesUserData Serialize()
        {
            return new HatcheriesUserData(this);
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

        private int CalculateUsedCapacity()
        {
            var currentPigeons = _userStateModel.CurrentPigeons.Value;
            
            if (currentPigeons < MaxCapacity.Value)
            {
                return _userStateModel.CurrentPigeons.Value;
            }
            return MaxCapacity.Value;
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
    }
}