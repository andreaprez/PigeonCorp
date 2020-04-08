using PigeonCorp.Commands;
using PigeonCorp.MainBuyButton;
using PigeonCorp.UserState;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;

namespace PigeonCorp.MainScreen
{
    public class MainInstaller : MonoBehaviour
    {
        [SerializeField] private PigeonView _pigeonPrefab;
        [SerializeField] private Transform _pigeonContainer;
        [Space]
        [SerializeField] private MainBuyButtonInstaller _mainBuyButtonInstaller;
        [Space]
        [SerializeField] private TitleDataHolder titleDataHolder;

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
            
            // GAME INIT
            
            var pigeonFactory = new PigeonFactory(_pigeonPrefab, _pigeonContainer);

            var userStateModel = new UserStateModel(userStateData);
            
            var mainBuyButtonModel = new MainBuyButtonModel(mainBuyButtonConfig);
            // TODO: Get multiplier from BonusModel
            var buyPigeonCommand = new BuyPigeonCommand(1, userStateModel, pigeonFactory);
            _mainBuyButtonInstaller.Install(mainBuyButtonModel, buyPigeonCommand);
        }
    }
}