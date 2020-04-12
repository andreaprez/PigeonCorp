using System.Collections.Generic;
using PigeonCorp.Bonus;
using PigeonCorp.Commands;
using PigeonCorp.Hatchery;
using PigeonCorp.MainBuyButton;
using PigeonCorp.MainTopBar;
using PigeonCorp.UserState;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
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
        [Space]
        [SerializeField] private PigeonView _pigeonPrefab;
        [SerializeField] private Transform _pigeonContainer;
        [SerializeField] private List<Transform> _pigeonRoutePoints;

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
                initUserCommand.Handle();
            }
            
            // TITLE DATA RETRIEVING
            var pigeonConfig = Gateway.Instance.GetPigeonConfig();
            var hatcheriesConfig = Gateway.Instance.GetHatcheriesConfig();

            // USER DATA RETRIEVING
            var userStateData = Gateway.Instance.GetUserStateData();
            var hatcheriesData = Gateway.Instance.GetHatcheriesData();
            
            // GAME INIT
            
            var pigeonFactory = new PigeonFactory(
                _pigeonPrefab,
                _pigeonContainer,
                _pigeonRoutePoints,
                pigeonConfig
            );

            var userStateModel = new UserStateModel(userStateData);
            
            var subtractCurrencyCommand = new SubtractCurrencyCommand(userStateModel);
            
            // TODO: Init BonusModel with all to 1
            var bonusModel = new BonusModel();
            
            var mainBuyButtonModel = new MainBuyButtonModel(bonusModel);
            var buyPigeonCommand = new BuyPigeonCommand(
                mainBuyButtonModel,
                userStateModel,
                pigeonFactory,
                pigeonConfig,
                subtractCurrencyCommand
            );
            _mainBuyButtonInstaller.Install(
                mainBuyButtonModel,
                buyPigeonCommand,
                userStateModel,
                pigeonConfig
            );

            var mainTopBarModel = new MainTopBarModel(userStateModel);
            _mainTopBarInstaller.Install(mainTopBarModel, userStateModel);
            
            var hatcheriesModel = new HatcheriesModel(hatcheriesConfig, hatcheriesData, userStateModel);
            _hatcheriesInstaller.Install(
                hatcheriesModel,
                hatcheriesConfig,
                userStateModel,
                subtractCurrencyCommand
            );
        }
    }
}