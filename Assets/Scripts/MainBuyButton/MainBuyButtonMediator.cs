using PigeonCorp.Dispatcher;
using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research;
using PigeonCorp.UserState;
using PigeonCorp.ValueModifiers;
using UniRx;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonMediator
    {
        private readonly MainBuyButtonModel _model;
        private readonly BuyButtonValueModifiers _valueModifiers;

        public MainBuyButtonMediator(
            MainBuyButtonView view,
            MainBuyButtonModel model,
            ICommand<int> buyPigeonCommand,
            UserStateModel userStateModel,
            PigeonTitleData pigeonConfig,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUc
        )
        {
            _model = model;
            
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
                buyPigeonCommand.Execute(model.PigeonsPerClick);
            }).AddTo(MainDispatcher.Disposables);

            _valueModifiers = (BuyButtonValueModifiers)getMainBuyButtonModifiersUc.Execute();
            SubscribeToValueModifiers();
        }

        private void SubscribeToValueModifiers()
        {
            _valueModifiers.PigeonsPerClickMultiplier.Subscribe(multiplier =>
            {
                _model.PigeonsPerClick = (int)multiplier;
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}