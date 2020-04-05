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
            ICommand<int> buyPigeonCommand
        )
        {
            view.GetButtonAsObservable().Subscribe(onClick =>
            {
                buyPigeonCommand.Handle(model.PigeonsPerClick);
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}