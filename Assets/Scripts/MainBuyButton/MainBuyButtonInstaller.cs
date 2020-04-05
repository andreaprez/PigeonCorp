using PigeonCorp.Commands;
using UnityEngine;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonInstaller : MonoBehaviour
    {
        [SerializeField] private MainBuyButtonView _view;
        
        public void Install(MainBuyButtonModel model, ICommand<int> buyPigeonCommand)
        {
            var mediator = new MainBuyButtonMediator(_view, model, buyPigeonCommand);
        }
    }
}