using PigeonCorp.Dispatcher;
using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using UniRx;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonMediator
    {
        public MainBuyButtonMediator(
            MainBuyButtonView view,
            MainBuyButtonModel model,
            ICommand<int> buyPigeonCommand,
            UserStateModel userStateModel,
            PigeonTitleData pigeonConfig
        )
        {
            userStateModel.Currency.AsObservable().Subscribe(currency =>
            {
                var nextClickCost = model.PigeonsPerClick * pigeonConfig.Cost;
                if (currency < nextClickCost)
                {
                    view.SetButtonInteractable(false);
                }
                else
                {
                    view.SetButtonInteractable(true);
                }
            }).AddTo(MainDispatcher.Disposables);
            
            view.GetButtonAsObservable().Subscribe(onClick =>
            {
                buyPigeonCommand.Handle(model.PigeonsPerClick);
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}