using System.Collections.Generic;
using PigeonCorp.Installers.Shipping.UseCase;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Shipping;
using UnityEngine;

namespace PigeonCorp.Factory
{
    public class VehicleFactory : IFactory<int>
    {
        private readonly List<VehicleBehaviour> _vehiclePrefabs;
        private readonly Transform _container;
        private readonly ShippingTitleData _config;

        public VehicleFactory(
            ShippingTitleData config,
            UC_GetVehiclePrefabs getVehiclePrefabsUC,
            UC_GetVehicleContainer getVehicleContainerUC
        )
        {
            _config = config;
            _vehiclePrefabs = getVehiclePrefabsUC.Execute();
            _container = getVehicleContainerUC.Execute();
        }
        
        public void Create(int prefabId)
        {
            VehicleBehaviour vehicle = Object.Instantiate(_vehiclePrefabs[prefabId], _container);
            vehicle.Initialize(_config);
        }
    }
}