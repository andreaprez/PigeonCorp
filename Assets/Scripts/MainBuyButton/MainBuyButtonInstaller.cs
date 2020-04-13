using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Factory;
using PigeonCorp.MainScreen;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonInstaller : MonoBehaviour
    {
        [SerializeField] private MainBuyButtonView _view;
        [Space]
        [SerializeField] private PigeonBehaviour _pigeonPrefab;
        [SerializeField] private Transform _pigeonContainer;
        [SerializeField] private List<Transform> _pigeonRoutePoints;
        
        public void Install(
            MainBuyButtonModel model,
            UserStateModel userStateModel,
            PigeonTitleData pigeonConfig,
            ICommand<float> subtractCurrencyCommand
        )
        {
            var pigeonFactory = new PigeonFactory(
                _pigeonPrefab,
                _pigeonContainer,
                _pigeonRoutePoints,
                pigeonConfig
            );
            var spawnPigeonCommand = new SpawnPigeonCommand(userStateModel, pigeonFactory);
            var buyPigeonCommand = new BuyPigeonCommand(
                spawnPigeonCommand,
                pigeonConfig,
                subtractCurrencyCommand
            );
            
            var mediator = new MainBuyButtonMediator(
                _view,
                model,
                buyPigeonCommand,
                userStateModel,
                pigeonConfig
            );
        }
    }
}