using System.Collections.Generic;
using PigeonCorp.Commands;
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
            
            // USER DATA RETRIEVING
            var userStateData = Gateway.Instance.GetUserStateData();
            
            // TITLE DATA RETRIEVING
            var mainBuyButtonConfig = Gateway.Instance.GetMainBuyButtonConfig();
            var pigeonConfig = Gateway.Instance.GetPigeonConfig();
            
            // GAME INIT
            
            var pigeonFactory = new PigeonFactory(
                _pigeonPrefab,
                _pigeonContainer,
                _pigeonRoutePoints,
                pigeonConfig
            );

            var userStateModel = new UserStateModel(userStateData);
            
            var mainBuyButtonModel = new MainBuyButtonModel(mainBuyButtonConfig);
            // TODO: Get multiplier from BonusModel
            var buyPigeonCommand = new BuyPigeonCommand(
                mainBuyButtonModel,
                userStateModel,
                pigeonFactory,
                pigeonConfig,
                1
            );
            _mainBuyButtonInstaller.Install(mainBuyButtonModel, buyPigeonCommand);

            var mainTopBarModel = new MainTopBarModel(userStateModel);
            _mainTopBarInstaller.Install(mainTopBarModel, userStateModel);
        }
    }
}