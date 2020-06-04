using System.Collections.Generic;
using Evolution.UseCase;
using PigeonCorp.Evolution.Adapter;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Hatcheries.Adapter;
using PigeonCorp.Hatcheries.Entity;
using PigeonCorp.Hatcheries.UseCase;
using PigeonCorp.MainBuyButton.Adapter;
using PigeonCorp.MainBuyButton.Entity;
using PigeonCorp.MainBuyButton.UseCase;
using PigeonCorp.MainScreen.Framework;
using PigeonCorp.MainTopBar.Adapter;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.MainTopBar.UseCase;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research.Adapter;
using PigeonCorp.Research.Entity;
using PigeonCorp.Shipping.Adapter;
using PigeonCorp.Shipping.Entity;
using PigeonCorp.Shipping.Framework;
using PigeonCorp.Shipping.UseCase;
using PigeonCorp.ValueModifiers.UseCase;
using UnityEngine;

namespace PigeonCorp.GameInstallation
{
    public class MainInstaller : MonoBehaviour
    {
        [SerializeField] private TitleDataHolder titleDataHolder;
        [Space]
        [SerializeField] private Transform _pigeonsContainer;
        [SerializeField] private List<PigeonBehaviour> _pigeonPrefabs;
        [SerializeField] private List<Transform> _pigeonDestinations;
        [Space]
        [SerializeField] private List<GameObject> _hatcheryPrefabs;
        [SerializeField] private List<Transform> _hatcheryContainers;
        [Space]
        [SerializeField] private List<VehicleBehaviour> _vehiclePrefabs;
        [SerializeField] private Transform _vehicleContainer;
        
        private void Start()
        {
            InitializeGateway();
            InitializeUser();
            InitializeGame();
        }

        private void InitializeGateway()
        {
            var userDataGateway = new BinaryGateway();
            var titleDataGateway = new ScriptableObjectGateway(titleDataHolder);
            Gateway.Instance.Initialize(titleDataGateway, userDataGateway);
        }
        
        private void InitializeUser()
        {
            var isInitialized = Gateway.Instance.GetUserInitialized();
            if (isInitialized == null)
            {
                var initUserCommand = new InitializeUserCommand();
                initUserCommand.Execute();
            }
        }
        
        private void InitializeGame()
        {
            var pigeonConfig = Gateway.Instance.GetPigeonConfig();
            var hatcheriesConfig = Gateway.Instance.GetHatcheriesConfig();
            var shippingConfig = Gateway.Instance.GetShippingConfig();
            var researchConfig = Gateway.Instance.GetResearchConfig();
            var evolutionConfig = Gateway.Instance.GetEvolutionConfig();

            var userStateData = Gateway.Instance.GetUserStateData();
            var hatcheriesData = Gateway.Instance.GetHatcheriesData();
            var shippingData = Gateway.Instance.GetShippingData();
            var researchData = Gateway.Instance.GetResearchData();
            var evolutionData = Gateway.Instance.GetEvolutionData();


            var getPigeonPrefabsUC = new UC_GetPigeonPrefabs(_pigeonPrefabs);
            var getPigeonContainerUC = new UC_GetPigeonsContainer(_pigeonsContainer);
            var getPigeonDestinationsUC = new UC_GetPigeonDestinations(_pigeonDestinations);

            var getHatcheryPrefabsUC = new UC_GetHatcheryPrefabs(_hatcheryPrefabs);
            var getHatcheryContainersUC = new UC_GetHatcheriesContainers(_hatcheryContainers);

            var getVehiclePrefabsUC = new UC_GetVehiclePrefabs(_vehiclePrefabs);
            var getVehicleContainerUC = new UC_GetVehicleContainer(_vehicleContainer);


            var initValueModifiersRepositoryUC = new UC_InitValueModifiersRepository();
            var valueModifiersRepository = initValueModifiersRepositoryUC.Execute();

            var getMainBuyButtonModifiersUC = new UC_GetMainBuyButtonValueModifiers(valueModifiersRepository);
            var getHatcheriesModifiersUC = new UC_GetHatcheriesValueModifiers(valueModifiersRepository);
            var getShippingModifiersUC = new UC_GetShippingValueModifiers(valueModifiersRepository);
            var getResearchModifiersUC = new UC_GetResearchValueModifiers(valueModifiersRepository);
            var getEvolutionModifiersUC = new UC_GetEvolutionValueModifiers(valueModifiersRepository);


            var mainTopBarEntity = new MainTopBarEntity();
            new MainTopBarInstaller().Install(mainTopBarEntity, userStateData);

            var subtractCurrencyCommand = new SubtractCurrencyCommand(mainTopBarEntity);
            var addCurrencyCommand = new AddCurrencyCommand(mainTopBarEntity);


            var evolutionEntity = new EvolutionEntity();
            var resetFarmCommand = new ResetFarmCommand(evolutionEntity);
            new EvolutionInstaller().Install(
                evolutionEntity,
                evolutionData,
                evolutionConfig,
                resetFarmCommand,
                getEvolutionModifiersUC
            );


            var hatcheriesEntity = new HatcheriesEntity();
            var hatcheryFactory = new HatcheryFactory(
                getHatcheryPrefabsUC,
                getHatcheryContainersUC
            );
            var spawnHatcheryCommand = new SpawnHatcheryCommand(hatcheryFactory);
            var getRandomBuiltHatcheryIdUC = new UC_GetRandomBuiltHatcheryId(hatcheriesEntity);
            new HatcheriesInstaller().Install(
                hatcheriesEntity,
                hatcheriesData,
                hatcheriesConfig,
                mainTopBarEntity,
                subtractCurrencyCommand,
                spawnHatcheryCommand,
                getHatcheriesModifiersUC
            );


            var shippingEntity = new ShippingEntity();
            var vehicleFactory = new VehicleFactory(
                shippingConfig,
                getVehiclePrefabsUC,
                getVehicleContainerUC
            );
            var spawnVehicleCommand = new SpawnVehicleCommand(vehicleFactory);
            var grantShippingRevenueCommand = new GrantShippingRevenueCommand(
                addCurrencyCommand,
                evolutionEntity,
                shippingEntity
            );
            new ShippingInstaller().Install(
                shippingEntity,
                shippingData,
                shippingConfig,
                hatcheriesEntity,
                mainTopBarEntity,
                subtractCurrencyCommand,
                grantShippingRevenueCommand,
                spawnVehicleCommand,
                getShippingModifiersUC
            );

            
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
                getResearchModifiersUC,
                getEvolutionModifiersUC
            );
            

            var mainBuyButtonEntity = new MainBuyButtonEntity();
            var pigeonFactory = new PigeonFactory(
                pigeonConfig,
                getPigeonPrefabsUC,
                getPigeonContainerUC,
                getPigeonDestinationsUC,
                getRandomBuiltHatcheryIdUC,
                evolutionEntity
            );
            var spawnPigeonCommand = new SpawnPigeonCommand(mainTopBarEntity, pigeonFactory);
            new MainBuyButtonInstaller().Install(
                mainBuyButtonEntity,
                mainTopBarEntity,
                pigeonConfig,
                subtractCurrencyCommand,
                spawnPigeonCommand,
                getMainBuyButtonModifiersUC
            );


            var grantOfflineRevenueCommand = new GrantOfflineRevenueCommand(
                addCurrencyCommand,
                evolutionEntity,
                shippingEntity,
                mainTopBarEntity
            );
            grantOfflineRevenueCommand.Execute();
        }
    }
}