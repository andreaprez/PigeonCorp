using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Factory;
using PigeonCorp.Hatcheries;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class ShippingInstaller : MonoBehaviour
    {
        [SerializeField] private ShippingView _view;
        [Space]
        [SerializeField] private List<VehicleBehaviour> _vehiclePrefabs;
        [SerializeField] private Transform _vehicleContainer;

        public void Install(
            ShippingModel model,
            ShippingTitleData config,
            HatcheriesModel hatcheriesModel,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand,
            ICommand grantShippingRevenueCommand
        )
        {
            var vehicleFactory = new VehicleFactory(
                _vehiclePrefabs,
                _vehicleContainer,
                config
            );
            var spawnVehicleCommand = new SpawnVehicleCommand(vehicleFactory);

            var mediator = new ShippingMediator(
                model,
                _view,
                config,
                hatcheriesModel,
                userStateModel,
                subtractCurrencyCommand,
                spawnVehicleCommand,
                grantShippingRevenueCommand
            );
        }
    }
}