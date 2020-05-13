using PigeonCorp.Research;
using PigeonCorp.Commands;
using PigeonCorp.Hatcheries;
using PigeonCorp.MainBuyButton;
using PigeonCorp.MainTopBar;
using PigeonCorp.UserState;
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
        [SerializeField] private MainBuyButtonInstaller _mainBuyButtonInstaller;
        [SerializeField] private MainTopBarInstaller _mainTopBarInstaller;
        [SerializeField] private HatcheriesInstaller _hatcheriesInstaller;
        [SerializeField] private ShippingInstaller _shippingInstaller;
        [SerializeField] private ResearchInstaller _researchInstaller;


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
            var userStateModel = new UserStateModel(userStateData);
            
            var subtractCurrencyCommand = new SubtractCurrencyCommand(userStateModel);
            var addCurrencyCommand = new AddCurrencyCommand(userStateModel);

            var initValueModifiersRepositoryUC = new UC_InitValueModifiersRepository();
            var valueModifiersRepository = initValueModifiersRepositoryUC.Execute();
            
            var getMainBuyButtonModifiersUC = new UC_GetMainBuyButtonValueModifiers(valueModifiersRepository);
            var getHatcheriesModifiersUC = new UC_GetHatcheriesValueModifiers(valueModifiersRepository);
            var getShippingModifiersUC = new UC_GetShippingValueModifiers(valueModifiersRepository);
            var getResearchModifiersUC = new UC_GetResearchValueModifiers(valueModifiersRepository);
            
            
            var mainTopBarModel = new MainTopBarModel(userStateModel);
            _mainTopBarInstaller.Install(mainTopBarModel, userStateModel);
            
            var researchModel = new ResearchModel(
                researchConfig,
                researchData,
                getMainBuyButtonModifiersUC.Execute(),
                getHatcheriesModifiersUC.Execute(),
                getShippingModifiersUC.Execute(),
                getResearchModifiersUC.Execute()
            );
            _researchInstaller.Install(
                researchModel,
                researchConfig,
                subtractCurrencyCommand,
                getResearchModifiersUC,
                getMainBuyButtonModifiersUC,
                userStateModel
            );
            
            var hatcheriesModel = new HatcheriesModel(hatcheriesConfig, hatcheriesData, userStateModel);
            _hatcheriesInstaller.Install(
                hatcheriesModel,
                hatcheriesConfig,
                userStateModel,
                subtractCurrencyCommand,
                getHatcheriesModifiersUC
            );

            var shippingModel = new ShippingModel(shippingConfig, shippingData, hatcheriesModel);
            var grantShippingRevenueCommand = new GrantShippingRevenueCommand(addCurrencyCommand, shippingModel);
            _shippingInstaller.Install(
                shippingModel,
                shippingConfig,
                hatcheriesModel,
                userStateModel,
                subtractCurrencyCommand,
                grantShippingRevenueCommand,
                getShippingModifiersUC
            );
            
            var mainBuyButtonModel = new MainBuyButtonModel(pigeonConfig);
            _mainBuyButtonInstaller.Install(
                mainBuyButtonModel,
                userStateModel,
                pigeonConfig,
                subtractCurrencyCommand,
                hatcheriesModel,
                getMainBuyButtonModifiersUC
            );
        }
    }
}