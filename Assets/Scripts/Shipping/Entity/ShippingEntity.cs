using System.Collections.Generic;
using PigeonCorp.Hatcheries;
using PigeonCorp.Utils;
using UniRx;

namespace PigeonCorp.Shipping
{
    public class ShippingEntity
    {
        public HatcheriesEntity HatcheriesEntity;
        
        public readonly List<VehicleEntity> Vehicles;
        public readonly ReactiveProperty<float> MaxShippingRate;
        public readonly ReactiveProperty<float> UsedShippingRate;
        
        public ShippingEntity()
        {
            Vehicles = new List<VehicleEntity>();
            MaxShippingRate = new ReactiveProperty<float>();
            UsedShippingRate = new ReactiveProperty<float>();
        }

        public void UpdateMaxShippingRate()
        {
            MaxShippingRate.Value = CalculateMaxShippingRate();
        }

        public void UpdateUsedShippingRate()
        {
            UsedShippingRate.Value = CalculateUsedShippingRate();
        }
        
        public void UpdateUsedShippingRateOfAllVehicles()
        {
            foreach (var vehicle in Vehicles)
            {
                var percentageOfTotalShippingRate = MathUtils.CalculatePercentageDecimalFromQuantity(
                    vehicle.MaxShippingRate.Value,
                    MaxShippingRate.Value
                );
                var usedQuantityFromPercentage =
                    percentageOfTotalShippingRate * UsedShippingRate.Value;

                vehicle.UsedShippingRate.Value = (int)usedQuantityFromPercentage;
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
            var totalProduction = HatcheriesEntity.TotalProduction.Value;
            
            if (totalProduction < MaxShippingRate.Value)
            {
                return HatcheriesEntity.TotalProduction.Value;
            }
            return MaxShippingRate.Value;
        }
    }
}