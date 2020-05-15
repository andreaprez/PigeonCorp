using System.Collections.Generic;
using PigeonCorp.Hatcheries;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using UniRx;

namespace PigeonCorp.Shipping
{
    public class ShippingModel
    {
        public readonly List<VehicleModel> Vehicles;
        public readonly ReactiveProperty<float> MaxShippingRate;
        public readonly ReactiveProperty<float> UsedShippingRate;

        private readonly ShippingTitleData _config;
        private readonly HatcheriesEntity _hatcheriesEntity;

        public ShippingModel(
            ShippingTitleData config,
            ShippingUserData userData,
            HatcheriesEntity hatcheriesEntity
        )
        {
            _config = config;
            _hatcheriesEntity = hatcheriesEntity;

            Vehicles = new List<VehicleModel>();
            InitVehicles(userData.Vehicles);
            
            MaxShippingRate = new ReactiveProperty<float>(CalculateMaxShippingRate());
            UsedShippingRate = new ReactiveProperty<float> (CalculateUsedShippingRate());
        }
        
        public void UpdateMaxShippingRate()
        {
            MaxShippingRate.Value = CalculateMaxShippingRate();
        }

        public void UpdateUsedShippingRate()
        {
            UsedShippingRate.Value = CalculateUsedShippingRate();
        }

        private void InitVehicles(List<VehicleState> vehiclesData)
        {
            for (int i = 0; i < vehiclesData.Count; i++)
            {
                var vehicle = new VehicleModel(_config, vehiclesData[i]);
                Vehicles.Add(vehicle);
            }
        }
        
        private float CalculateMaxShippingRate()
        {
            var maxShippingRate = 0f;
            
            foreach (var vehicle in Vehicles)
            {
                if (vehicle.Purchased.Value)
                {
                    maxShippingRate += vehicle.MaxShippingRate.Value;
                }
            }

            return maxShippingRate;
        }

        private float CalculateUsedShippingRate()
        {
            var totalProduction = _hatcheriesEntity.TotalProduction.Value;
            
            if (totalProduction < MaxShippingRate.Value)
            {
                return _hatcheriesEntity.TotalProduction.Value;
            }
            return MaxShippingRate.Value;
        }

        public ShippingUserData Serialize()
        {
            return new ShippingUserData(this);
        }
    }
}