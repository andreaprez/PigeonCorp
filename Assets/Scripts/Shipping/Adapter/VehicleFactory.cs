using System.Collections.Generic;
using PigeonCorp.Factory;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Shipping.Framework;
using PigeonCorp.Shipping.UseCase;
using UnityEngine;

namespace PigeonCorp.Shipping.Adapter
{
    public class VehicleFactory : IFactory<int>
    {
        private readonly List<VehicleBehaviour> _vehiclePrefabs;
        private List<VehicleBehaviour> _vehicleInstances;
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

            InitializePrefabInstances();
        }

        private void InitializePrefabInstances()
        {
            _vehicleInstances = new List<VehicleBehaviour>();
            
            for (int i = 0; i < _vehiclePrefabs.Count; i++)
            {
                VehicleBehaviour vehicle = Object.Instantiate(_vehiclePrefabs[i], _container);
                vehicle.gameObject.SetActive(false);
                _vehicleInstances.Add(vehicle);
            }
        }

        public void Create(int prefabId)
        {
            VehicleBehaviour vehicle = _vehicleInstances[prefabId];
            vehicle.transform.position = _container.position;
            vehicle.transform.rotation = _container.rotation;
            vehicle.gameObject.SetActive(true);
            vehicle.Initialize(_config);
        }
    }
}