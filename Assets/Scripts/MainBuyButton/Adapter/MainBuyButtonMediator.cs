using PigeonCorp.Command;
using PigeonCorp.Dispatcher;
using PigeonCorp.Hatcheries.Entity;
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
        private MainTopBarEntity _mainTopBarEntity;
        private HatcheriesEntity _hatcheriesEntity;
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
            PigeonTitleData pigeonConfig,
            MainTopBarEntity mainTopBarEntity,
            HatcheriesEntity hatcheriesEntity,
            ICommand spawnPigeonCommand,
            ICommand<float> subtractCurrencyCommand,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC
        )
        {
            _entity = entity;
            _mainTopBarEntity = mainTopBarEntity;
            _hatcheriesEntity = hatcheriesEntity;
            _pigeonConfig = pigeonConfig;
            _spawnPigeonCommand = spawnPigeonCommand;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _valueModifiers = (MainBuyButtonValueModifiers)getMainBuyButtonModifiersUC.Execute();
            
            SubscribeToCurrency();
            SubscribeToHatcheriesCapacity();
            SubscribeToValueModifiers();
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
        
        private void SubscribeToCurrency()
        {
            _mainTopBarEntity.Currency.AsObservable().Subscribe(currency =>
            {
                _viewModel.IsInteractable.Value = CheckCanBuyPigeon();
            }).AddTo(MainDispatcher.Disposables);
        }
        
        private void SubscribeToHatcheriesCapacity()
        {
            _hatcheriesEntity.MaxCapacity.AsObservable().Subscribe(maxCap =>
            {
                _viewModel.IsInteractable.Value = CheckCanBuyPigeon();
            }).AddTo(MainDispatcher.Disposables);
            _mainTopBarEntity.PigeonsCount.AsObservable().Subscribe(count =>
            {
                _viewModel.IsInteractable.Value = CheckCanBuyPigeon();
            }).AddTo(MainDispatcher.Disposables);
        }

        private bool CheckCanBuyPigeon()
        {
            var enoughRoom = CheckHasRoomForMorePigeons();
            var enoughCurrency = CheckHasEnoughCurrency();
            var canBuy = enoughRoom && enoughCurrency;
            return canBuy;
        }

        private bool CheckHasRoomForMorePigeons()
        {
            var currentPigeonsCount = _mainTopBarEntity.PigeonsCount.Value;
            var maxCapacity = _hatcheriesEntity.MaxCapacity.Value;
            var hasRoomForMorePigeons = currentPigeonsCount + _entity.PigeonsPerClick <= maxCapacity;
            return hasRoomForMorePigeons;
        }
        
        private bool CheckHasEnoughCurrency()
        {
            var currency = _mainTopBarEntity.Currency.Value;
            var nextClickCost = _entity.PigeonsPerClick * _pigeonConfig.Cost;
            var enoughCurrency = currency >= nextClickCost;
            return enoughCurrency;
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