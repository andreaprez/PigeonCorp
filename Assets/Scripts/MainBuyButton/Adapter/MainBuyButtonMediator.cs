using PigeonCorp.Command;
using PigeonCorp.Dispatcher;
using PigeonCorp.MainBuyButton.Entity;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers.Entity;
using PigeonCorp.ValueModifiers.UseCase;
using UniRx;
using Zenject;

namespace PigeonCorp.MainBuyButton.Adapter
{
    public class MainBuyButtonMediator
    {
        private readonly MainBuyButtonViewModel _viewModel;

        private MainBuyButtonEntity _entity;
        private PigeonTitleData _pigeonConfig;
        private ICommand _spawnPigeonCommand;
        private ICommand<float> _subtractCurrencyCommand;
        private MainBuyButtonValueModifiers _valueModifiers;
        
        public MainBuyButtonMediator()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<MainBuyButtonViewModel>();
        }

        public void Initialize(
            MainBuyButtonEntity entity,
            ICommand spawnPigeonCommand,
            ICommand<float> subtractCurrencyCommand,
            MainTopBarEntity mainTopBarEntity,
            PigeonTitleData pigeonConfig,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC
        )
        {
            _entity = entity;
            _pigeonConfig = pigeonConfig;
            _spawnPigeonCommand = spawnPigeonCommand;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _valueModifiers = (MainBuyButtonValueModifiers)getMainBuyButtonModifiersUC.Execute();
            
            SubscribeToCurrency(mainTopBarEntity);
            SubscribeToValueModifiers();
        }

        private void SubscribeToCurrency(MainTopBarEntity mainTopBarEntity)
        {
            mainTopBarEntity.Currency.AsObservable().Subscribe(currency =>
            {
                var nextClickCost = _entity.PigeonsPerClick * _pigeonConfig.Cost;
                if (currency < nextClickCost)
                {
                    _viewModel.IsInteractable.Value = false;
                }
                else
                {
                    _viewModel.IsInteractable.Value = true;
                }
            }).AddTo(MainDispatcher.Disposables);
        }

        public void OnButtonClick()
        {
            var cost = _entity.PigeonCost * _entity.PigeonsPerClick;
            _subtractCurrencyCommand.Execute(cost);

            for (int i = 0; i < _entity.PigeonsPerClick; i++)
            {
                _spawnPigeonCommand.Execute();
            }
        }

        private void SubscribeToValueModifiers()
        {
            _valueModifiers.PigeonsPerClickMultiplier
                .Subscribe(ApplyMultiplierToBuyRate)
                .AddTo(MainDispatcher.Disposables);
            
            _valueModifiers.PigeonCostDiscount
                .Subscribe(ApplyDiscountToPigeon)
                .AddTo(MainDispatcher.Disposables);
        }

        private void ApplyMultiplierToBuyRate(float multiplier)
        {
            _entity.PigeonsPerClick = (int)multiplier;
        }

        private void ApplyDiscountToPigeon(float discount)
        {
            var baseValue = _pigeonConfig.Cost;
            var discountValue = MathUtils.CalculateQuantityFromPercentage(
                discount,
                baseValue
            );
            _entity.PigeonCost = baseValue - discountValue;
        }
    }
}