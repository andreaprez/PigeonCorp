using System.Collections.Generic;
using PigeonCorp.Research;
using PigeonCorp.Commands;
using PigeonCorp.Hatcheries;
using PigeonCorp.Installers.Hatcheries.UseCase;
using PigeonCorp.Installers.Shipping.UseCase;
using PigeonCorp.MainBuyButton;
using PigeonCorp.MainScreen.UseCase;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Shipping;
using PigeonCorp.ValueModifiers;
using UnityEngine;

namespace PigeonCorp.MainScreen
{
    public class MainInstaller : MonoBehaviour
    {
        [SerializeField] private TitleDataHolder titleDataHolder;
        [Space]
        [SerializeField] private Transform _pigeonsContainer;
        [SerializeField] private List<Transform> _pigeonDestinations;
        [Space]
        [SerializeField] private List<GameObject> _hatcheryPrefabs;
        [SerializeField] private List<Transform> _hatcheryContainers;
        [Space]
        [SerializeField] private List<VehicleBehaviour> _vehiclePrefabs;
        [SerializeField] private Transform _vehicleContainer;

        private void Start()
        {
            // GATEWAY INITIALIZATION
            var userDataGateway = new BinaryGateway();
            var titleDataGateway = new ScriptableObjectGateway(titleDataHolder);
            Gateway.Instance.Initialize(titleDataGateway, userDataGateway);
            
            // USER INITIALIZATION
            var isInitialized = Gateway.Instance.GetUserInitialized();
            if (isInitialized == null)
            {
                var initUserCommand = new InitializeUserCommand();
                initUserCommand.Execute();
            }
            
            // TITLE DATA RETRIEVING
            var pigeonConfig = Gateway.Instance.GetPigeonConfig();
            var hatcheriesConfig = Gateway.Instance.GetHatcheriesConfig();
            var shippingConfig = Gateway.Instance.GetShippingConfig();
            var researchConfig = Gateway.Instance.GetResearchConfig();

            // USER DATA RETRIEVING
            var userStateData = Gateway.Instance.GetUserStateData();
            var hatcheriesData = Gateway.Instance.GetHatcheriesData();
            var shippingData = Gateway.Instance.GetShippingData();
            var researchData = Gateway.Instance.GetResearchData();
            
            // GAME INIT
            var mainTopBarEntity = new MainTopBarEntity();
            new MainTopBarInstaller().Install(mainTopBarEntity, userStateData);
            
            var subtractCurrencyCommand = new SubtractCurrencyCommand(mainTopBarEntity);
            var addCurrencyCommand = new AddCurrencyCommand(mainTopBarEntity);

            var initValueModifiersRepositoryUC = new UC_InitValueModifiersRepository();
            var valueModifiersRepository = initValueModifiersRepositoryUC.Execute();
            
            var getPigeonsContainerUC = new UC_GetPigeonsContainer(_pigeonsContainer);
            var getPigeonDestinationsUC = new UC_GetPigeonDestinations(_pigeonDestinations);
            
            var getHatcheriesPrefabsUC = new UC_GetHatcheryPrefabs(_hatcheryPrefabs);
            var getHatcheriesContainersUC = new UC_GetHatcheriesContainers(_hatcheryContainers);
            
            var getVehiclesPrefabsUC = new UC_GetVehiclePrefabs(_vehiclePrefabs);
            var getVehicleContainerUC = new UC_GetVehicleContainer(_vehicleContainer);

            var getMainBuyButtonModifiersUC = new UC_GetMainBuyButtonValueModifiers(valueModifiersRepository);
            var getHatcheriesModifiersUC = new UC_GetHatcheriesValueModifiers(valueModifiersRepository);
            var getShippingModifiersUC = new UC_GetShippingValueModifiers(valueModifiersRepository);
            var getResearchModifiersUC = new UC_GetResearchValueModifiers(valueModifiersRepository);
            
            
            var researchEntity = new ResearchEntity();
            new ResearchInstaller().Install(
                researchEntity,
                researchData,
                researchConfig,
                mainTopBarEntity,
                subtractCurrencyCommand,
                getMainBuyButtonModifiersUC,
                getHatcheriesModifiersUC,
                getShippingModifiersUC,
                getResearchModifiersUC
            );
            
            var hatcheriesEntity = new HatcheriesEntity();
            new HatcheriesInstaller().Install(
                hatcheriesEntity,
                hatcheriesData,
                hatcheriesConfig,
                mainTopBarEntity,
                subtractCurrencyCommand,
                getHatcheriesPrefabsUC,
                getHatcheriesContainersUC,
                getHatcheriesModifiersUC
            );

            var shippingEntity = new ShippingEntity();
            var grantShippingRevenueCommand = new GrantShippingRevenueCommand(addCurrencyCommand, shippingEntity);
            new ShippingInstaller().Install(
                shippingEntity,
                shippingData,
                shippingConfig,
                hatcheriesEntity,
                mainTopBarEntity,
                subtractCurrencyCommand,
                grantShippingRevenueCommand,
                getVehiclesPrefabsUC,
                getVehicleContainerUC,
                getShippingModifiersUC
            );
            
            var mainBuyButtonEntity = new MainBuyButtonEntity();
            new MainBuyButtonInstaller().Install(
                mainBuyButtonEntity,
                mainTopBarEntity,
                pigeonConfig,
                subtractCurrencyCommand,
                hatcheriesEntity,
                getPigeonsContainerUC,
                getPigeonDestinationsUC,
                getMainBuyButtonModifiersUC
            );
        }
    }
}