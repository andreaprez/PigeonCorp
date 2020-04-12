using System.Collections.Generic;
using PigeonCorp.Hatchery;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using UniRx;

namespace PigeonCorp.Shipping
{
    public class ShippingModel
    {
        public readonly List<VehicleModel> Vehicles;
        public readonly ReactiveProperty<int> MaxShippingRate;
        public readonly ReactiveProperty<int> UsedShippingRate;

        private readonly ShippingTitleData _config;
        private readonly HatcheriesModel _hatcheriesModel;

        public ShippingModel(
            ShippingTitleData config,
            ShippingUserData userData,
            HatcheriesModel hatcheriesModel
        )
        {
            _config = config;
            _hatcheriesModel = hatcheriesModel;

            Vehicles = new List<VehicleModel>();
            InitVehicles(userData.Vehicles);
            
            MaxShippingRate = new ReactiveProperty<int>(CalculateMaxShippingRate());
            UsedShippingRate = new ReactiveProperty<int> (_hatcheriesModel.TotalProduction.Value);
        }

        public void UpdateUsedShippingRate(int production)
        {
            UsedShippingRate.Value = production;
        }

        public void UpdateMaxShippingRate()
        {
            MaxShippingRate.Value = CalculateMaxShippingRate();
        }

        private void InitVehicles(List<VehicleState> vehiclesData)
        {
            for (int i = 0; i < vehiclesData.Count; i++)
            {
                var vehicle = new VehicleModel(_config, vehiclesData[i]);
                Vehicles.Add(vehicle);
            }
        }
        
        private int CalculateMaxShippingRate()
        {
            var maxShippingRate = 0;
            
            foreach (var vehicle in Vehicles)
            {
                if (vehicle.Purchased.Value)
                {
                    maxShippingRate += vehicle.MaxShippingRate.Value;
                }
            }

            return maxShippingRate;
        }

        public ShippingUserData Serialize()
        {
            return new ShippingUserData(this);
        }
    }
}