using System;
using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Shipping.Entity;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class ShippingUserData
    {
        public readonly List<VehicleState> Vehicles;

        
        public ShippingUserData(ShippingTitleData config)
        {
            Vehicles = config.InitialVehicles;
        }
        
        public ShippingUserData(ShippingEntity entity)
        {
            Vehicles = new List<VehicleState>();
            
            for (int i = 0; i < entity.Vehicles.Count; i++)
            {
                var vehicleModel = entity.Vehicles[i];
                var vehicleData = new VehicleState
                {
                    Purchased = vehicleModel.Purchased.Value,
                    Level = vehicleModel.Level.Value
                };
                Vehicles.Add(vehicleData);
            }
        }
    }
}