using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Hatchery;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class ShippingInstaller : MonoBehaviour
    {
        [SerializeField] private ShippingView _view;
        [SerializeField] private List<VehicleBehaviour> _vehiclePrefabs;

        public void Install(
            ShippingModel model,
            ShippingTitleData config,
            HatcheriesModel hatcheriesModel,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand
        )
        {
            var mediator = new ShippingMediator(
                model,
                _view,
                config,
                hatcheriesModel,
                userStateModel,
                subtractCurrencyCommand,
                _vehiclePrefabs
            );
        }
    }
}