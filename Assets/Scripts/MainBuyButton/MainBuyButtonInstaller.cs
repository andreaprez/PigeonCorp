using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonInstaller : MonoBehaviour
    {
        [SerializeField] private MainBuyButtonView _view;
        
        public void Install(
            MainBuyButtonModel model,
            ICommand buyPigeonCommand,
            UserStateModel userStateModel,
            PigeonTitleData pigeonConfig
        )
        {
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