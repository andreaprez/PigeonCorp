using PigeonCorp.Dispatcher;
using PigeonCorp.Commands;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research;
using PigeonCorp.UserState;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonMediator
    {
        private readonly MainBuyButtonModel _model;
        private readonly PigeonTitleData _pigeonConfig;
        private readonly MainBuyButtonValueModifiers _valueModifiers;

        public MainBuyButtonMediator(
            MainBuyButtonView view,
            MainBuyButtonModel model,
            ICommand spawnPigeonCommand,
            ICommand<float> subtractCurrencyCommand,
            MainTopBarEntity mainTopBarEntity,
            PigeonTitleData pigeonConfig,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC
        )
        {
            _model = model;
            _pigeonConfig = pigeonConfig;

            _valueModifiers = (MainBuyButtonValueModifiers)getMainBuyButtonModifiersUC.Execute();
            
            mainTopBarEntity.Currency.AsObservable().Subscribe(currency =>
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
                var cost = _model.PigeonCost * _model.PigeonsPerClick;
                subtractCurrencyCommand.Execute(cost);

                for (int i = 0; i < _model.PigeonsPerClick; i++)
                {
                    spawnPigeonCommand.Execute();
                }
            }).AddTo(MainDispatcher.Disposables);

            SubscribeToValueModifiers();
        }

        private void SubscribeToValueModifiers()
        {
            _valueModifiers.PigeonsPerClickMultiplier.Subscribe(multiplier =>
            {
                ApplyMultiplierToBuyRate(multiplier);
            }).AddTo(MainDispatcher.Disposables);
            
            _valueModifiers.PigeonCostDiscount.Subscribe(discount =>
            {
                ApplyDiscountToPigeon(discount);
            }).AddTo(MainDispatcher.Disposables);
        }

        private void ApplyMultiplierToBuyRate(float multiplier)
        {
            _model.PigeonsPerClick = (int)multiplier;
        }

        private void ApplyDiscountToPigeon(float discount)
        {
            var baseValue = _pigeonConfig.Cost;
            var discountValue = MathUtils.CalculateQuantityFromPercentage(
                discount,
                baseValue
            );
            _model.PigeonCost = baseValue - discountValue;
        }
    }
}