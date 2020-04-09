using PigeonCorp.Dispatcher;
using PigeonCorp.Commands;
using UniRx;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonMediator
    {
        public MainBuyButtonMediator(
            MainBuyButtonView view,
            MainBuyButtonModel model,
            ICommand buyPigeonCommand
        )
        {
            // TODO: Set button no interactable if not enough currency
            
            view.GetButtonAsObservable().Subscribe(onClick =>
            {
                buyPigeonCommand.Handle();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}