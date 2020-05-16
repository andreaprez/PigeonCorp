using PigeonCorp.Command;
using PigeonCorp.Hatcheries.Entity;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.Shipping.Entity;
using PigeonCorp.Shipping.UseCase;
using PigeonCorp.ValueModifiers.UseCase;
using Zenject;

namespace PigeonCorp.Shipping.Adapter
{
    public class ShippingInstaller
    {
        public void Install(
            ShippingEntity entity,
            ShippingUserData data,
            ShippingTitleData config,
            HatcheriesEntity hatcheriesEntity,
            MainTopBarEntity mainTopBarEntity,
            ICommand<float> subtractCurrencyCommand,
            ICommand grantShippingRevenueCommand,
            UC_GetVehiclePrefabs getVehiclePrefabsUC,
            UC_GetVehicleContainer getVehicleContainerUC,
            UC_GetShippingValueModifiers getShippingValueModifiersUC
        )
        {
            InitEntity(entity, hatcheriesEntity, data, config);
            
            var vehicleFactory = new VehicleFactory(
                config,
                getVehiclePrefabsUC,
                getVehicleContainerUC
            );
            var spawnVehicleCommand = new SpawnVehicleCommand(vehicleFactory);
            
            ProjectContext.Instance.Container
                .Resolve<ShippingMediator>()
                .Initialize(
                    entity,
                    config,
                    hatcheriesEntity,
                    mainTopBarEntity,
                    subtractCurrencyCommand,
                    spawnVehicleCommand,
                    grantShippingRevenueCommand,
                    getShippingValueModifiersUC
            );
        }
        
        private void InitEntity(
            ShippingEntity entity,
            HatcheriesEntity hatcheriesEntity,
            ShippingUserData data,
            ShippingTitleData config
        )
        {
            entity.HatcheriesEntity = hatcheriesEntity;
            entity.UpdateUsedShippingRate();
            entity.UpdateMaxShippingRate();
            
            for (int i = 0; i < data.Vehicles.Count; i++)
            {
                var vehicle = new VehicleEntity();
                
                vehicle.Id = i;
                vehicle.Purchased.Value = data.Vehicles[i].Purchased;
                vehicle.Level.Value = data.Vehicles[i].Level;
                if (vehicle.Purchased.Value)
                {
                    vehicle.Name.Value = config.ShippingConfiguration[vehicle.Level.Value - 1].Name;
                    vehicle.Icon.Value = config.ShippingConfiguration[vehicle.Level.Value - 1].Icon;
                    vehicle.MaxShippingRate.Value = config.ShippingConfiguration[vehicle.Level.Value - 1].MaxShippingRate;
                }
                if (vehicle.Level.Value < config.ShippingConfiguration.Count)
                {
                    vehicle.NextCost.Value = config.ShippingConfiguration[vehicle.Level.Value].Cost;
                }

                entity.Vehicles.Add(vehicle);
            }
        }
    }
}