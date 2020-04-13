using System.Collections.Generic;
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
            List<VehicleBehaviour> prefabs,
            Transform container,
            ShippingTitleData config
        )
        {
            _vehiclePrefabs = prefabs;
            _container = container;
            _config = config;
        }
        
        public void Create(int prefabId)
        {
            VehicleBehaviour vehicle = Object.Instantiate(_vehiclePrefabs[prefabId], _container);
            vehicle.Initialize(_config);
        }
    }
}