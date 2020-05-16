using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;
using Zenject;

namespace PigeonCorp.Hatcheries
{
    public class HatcheryMediator
    {
        private HatcheriesEntity _hatcheriesEntity;
        private HatcheryEntity _entity;
        private HatcheriesTitleData _config;
        private MainTopBarEntity _mainTopBarEntity;
        private ICommand<int, int> _spawnHatcheryCommand;
        private HatcheriesValueModifiers _valueModifiers;
        private HatcheryViewModel _viewModel;

        public void Initialize(
            HatcheriesEntity hatcheriesEntity,
            HatcheryEntity entity,
            HatcheriesTitleData config,
            MainTopBarEntity mainTopBarEntity,
            ICommand<int, int> spawnHatcheryCommand,
            HatcheriesValueModifiers valueModifiers
        )
        {
            _hatcheriesEntity = hatcheriesEntity;
            _entity = entity;
            _config = config;
            _mainTopBarEntity = mainTopBarEntity;
            _spawnHatcheryCommand = spawnHatcheryCommand;
            _valueModifiers = valueModifiers;
            
            _viewModel = ProjectContext.Instance.Container
                .Resolve<HatcheriesViewModel>().HatcheryViewModels[_entity.Id];
            _viewModel.ButtonInteractable.Value = true;
            _viewModel.UpgradeAvailable.Value = true;

            SubscribeToCurrency();
            SubscribeToEntity();
            SubscribeToValueModifiers();
        }

        private void SubscribeToCurrency()
        {
            _mainTopBarEntity.Currency.AsObservable().Subscribe(currency =>
            {
                var nextCost = _entity.NextCost.Value;
                var enoughCurrency = currency >= nextCost;
                _viewModel.ButtonInteractable.Value = enoughCurrency;
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToEntity()
        {
            _entity.Built.AsObservable().Subscribe(built =>
            {
                _viewModel.Built.Value = built;
            }).AddTo(MainDispatcher.Disposables);

            _entity.Level.AsObservable().Subscribe(level =>
            {
                if (level > 0)
                {
                    _entity.Name.Value = _config.HatcheriesConfiguration[_entity.Level.Value - 1].Name;
                    _entity.Icon.Value = _config.HatcheriesConfiguration[_entity.Level.Value - 1].Icon;
                    _entity.MaxCapacity.Value = _config.HatcheriesConfiguration[_entity.Level.Value - 1].MaxCapacity;
                    _entity.EggLayingRate.Value =
                        _config.HatcheriesConfiguration[_entity.Level.Value - 1].EggLayingRate;
                    if (_entity.Level.Value < _config.HatcheriesConfiguration.Count)
                    {
                        _entity.NextCost.Value = _config.HatcheriesConfiguration[_entity.Level.Value].Cost;
                    }
                    else
                    {
                        _viewModel.UpgradeAvailable.Value = false;
                    }
                    
                    var prefabId = _entity.Level.Value - 1;
                    _spawnHatcheryCommand.Execute(prefabId, _entity.Id);
                }
                ApplyValueModifiers();
            }).AddTo(MainDispatcher.Disposables);

            _entity.Name.AsObservable().Subscribe(name =>
            {
                _viewModel.Name.Value = name;
            }).AddTo(MainDispatcher.Disposables);

            _entity.Icon.AsObservable().Subscribe(icon =>
            {
                _viewModel.Icon.Value = icon;
            }).AddTo(MainDispatcher.Disposables);

            _entity.NextCost.AsObservable().Subscribe(cost =>
            {
                _viewModel.Cost.Value = cost;
            }).AddTo(MainDispatcher.Disposables);

            _entity.UsedCapacity.AsObservable().Subscribe(usedCap =>
            {
                _hatcheriesEntity.UpdateTotalProduction();
                var percentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    usedCap,
                    _entity.MaxCapacity.Value
                );
                _viewModel.CapacityPercentage.Value = percentage;
            }).AddTo(MainDispatcher.Disposables);

            _entity.MaxCapacity.AsObservable().Subscribe(maxCap =>
            {
                _hatcheriesEntity.UpdateMaxCapacity();
                _hatcheriesEntity.UpdateUsedCapacityOfAllHatcheries();
                _viewModel.MaxCapacity.Value = maxCap;
            }).AddTo(MainDispatcher.Disposables);

            _entity.EggLayingRate.AsObservable().Subscribe(layingRate =>
            {
                _hatcheriesEntity.UpdateTotalProduction();
            })
            .AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToValueModifiers()
        {
            _valueModifiers.EggLayingRateMultiplier
                .Subscribe(ApplyMultiplierToEggLayingRate)
                .AddTo(MainDispatcher.Disposables);

            _valueModifiers.HatcheryCapacityIncrement
                .Subscribe(ApplyIncrementToMaxCapacity)
                .AddTo(MainDispatcher.Disposables);
            
            _valueModifiers.HatcheryCostDiscount
                .Subscribe(ApplyDiscountToHatchery)
                .AddTo(MainDispatcher.Disposables);
        }
        
        private void ApplyValueModifiers()
        {
            ApplyMultiplierToEggLayingRate(_valueModifiers.EggLayingRateMultiplier.Value);
            ApplyIncrementToMaxCapacity(_valueModifiers.HatcheryCapacityIncrement.Value);
            ApplyDiscountToHatchery(_valueModifiers.HatcheryCostDiscount.Value);
        }
        
        private void ApplyMultiplierToEggLayingRate(float multiplier)
        {
            if (_entity.Built.Value)
            { 
                var baseValue = _config.HatcheriesConfiguration[_entity.Level.Value - 1].EggLayingRate;
                _entity.EggLayingRate.Value = baseValue * multiplier;
            }
        }
        
        private void ApplyIncrementToMaxCapacity(float increment)
        {
            if (_entity.Built.Value)
            { 
                var baseValue = _config.HatcheriesConfiguration[_entity.Level.Value - 1].MaxCapacity;
                var incrementValue = MathUtils.CalculateQuantityFromPercentage(
                    increment,
                    baseValue
                );
                _entity.MaxCapacity.Value = baseValue + (int)incrementValue;
            }
        }
        
        private void ApplyDiscountToHatchery(float discount)
        {
            if (_entity.Level.Value < _config.HatcheriesConfiguration.Count)
            { 
                var baseValue = _config.HatcheriesConfiguration[_entity.Level.Value].Cost;
                var discountValue = MathUtils.CalculateQuantityFromPercentage(
                    discount,
                    baseValue
                );
                _entity.NextCost.Value = baseValue - discountValue;
            }
        }
    }
}