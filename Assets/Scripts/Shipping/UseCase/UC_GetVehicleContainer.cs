using UnityEngine;

namespace PigeonCorp.Shipping.UseCase
{
    public class UC_GetVehicleContainer
    {
        private readonly Transform _vehicleContainer;

        public UC_GetVehicleContainer(Transform vehicleContainer)
        {
            _vehicleContainer = vehicleContainer;
        }

        public Transform Execute()
        {
            return _vehicleContainer;
        }
    }
}