using PigeonCorp.Dispatcher;
using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research;
using PigeonCorp.UserState;
using UniRx;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonMediator
    {
        private readonly MainBuyButtonModel _model;

        public MainBuyButtonMediator(
            MainBuyButtonView view,
            MainBuyButtonModel model,
            ICommand<int> buyPigeonCommand,
            UserStateModel userStateModel,
            PigeonTitleData pigeonConfig,
            ResearchModel researchModel
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
                buyPigeonCommand.Handle(model.PigeonsPerClick);
            }).AddTo(MainDispatcher.Disposables);

            SubscribeToBonusValues(researchModel);
        }

        private void SubscribeToBonusValues(ResearchModel researchModel)
        {
            var buttonRateBonus = researchModel.GetBonusByType(BonusType.BUY_BUTTON_RATE);
            buttonRateBonus.CurrentValue.Subscribe(buttonRate =>
            {
                _model.PigeonsPerClick = (int) buttonRate;
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}