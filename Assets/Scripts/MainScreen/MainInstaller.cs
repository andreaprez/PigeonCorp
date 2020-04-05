using PigeonCorp.Commands;
using PigeonCorp.MainBuyButton;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.MainScreen
{
    public class MainInstaller : MonoBehaviour
    {
        [SerializeField] private PigeonView _pigeonPrefab;
        [SerializeField] private Transform _pigeonContainer;
        [Space]
        [SerializeField] private MainBuyButtonInstaller _mainBuyButtonInstaller;

        private void Start()
        {
            var pigeonFactory = new PigeonFactory(_pigeonPrefab, _pigeonContainer);

            var userStateModel = new UserStateModel();
            
            var mainBuyButtonModel = new MainBuyButtonModel();
            // TODO: Get multiplier from BonusModel
            var buyPigeonCommand = new BuyPigeonCommand(1, userStateModel, pigeonFactory);
            _mainBuyButtonInstaller.Install(mainBuyButtonModel, buyPigeonCommand);
        }
    }
}