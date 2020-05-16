using System.Collections.Generic;
using PigeonCorp.Shipping;

namespace PigeonCorp.Installers.Shipping.UseCase
{
    public class UC_GetVehiclePrefabs
    {
        private readonly List<VehicleBehaviour> _vehiclesPrefabs;

        public UC_GetVehiclePrefabs(List<VehicleBehaviour> vehiclesPrefabs)
        {
            _vehiclesPrefabs = vehiclesPrefabs;
        }

        public List<VehicleBehaviour> Execute()
        {
            return _vehiclesPrefabs;
        }
    }
}