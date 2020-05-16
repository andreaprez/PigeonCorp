using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;
using Zenject;

namespace PigeonCorp.Shipping
{
    public class VehicleMediator
    {
        private ShippingEntity _shippingEntity;
        private VehicleEntity _entity;
        private ShippingTitleData _config;
        private MainTopBarEntity _mainTopBarEntity;
        private ShippingValueModifiers _valueModifiers;
        private VehicleViewModel _viewModel;

        public void Initialize(
            ShippingEntity shippingEntity,
            VehicleEntity entity,
            ShippingTitleData config,
            MainTopBarEntity mainTopBarEntity,
            ShippingValueModifiers valueModifiers
        )
        {
            _shippingEntity = shippingEntity;
            _entity = entity;
            _config = config;
            _mainTopBarEntity = mainTopBarEntity;
            _valueModifiers = valueModifiers;
            
            _viewModel = ProjectContext.Instance.Container
                .Resolve<ShippingViewModel>().VehicleViewModels[_entity.Id];
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
            _entity.Purchased.AsObservable().Subscribe(purchased =>
            {
                _viewModel.Purchased.Value = purchased;
            }).AddTo(MainDispatcher.Disposables);

            _entity.Level.AsObservable().Subscribe(level =>
            {
                if (level > 0)
                {
                    _entity.Name.Value = _config.ShippingConfiguration[_entity.Level.Value - 1].Name;
                    _entity.Icon.Value = _config.ShippingConfiguration[_entity.Level.Value - 1].Icon;
                    _entity.MaxShippingRate.Value = _config.ShippingConfiguration[_entity.Level.Value - 1].MaxShippingRate;
                    if (_entity.Level.Value < _config.ShippingConfiguration.Count)
                    {
                        _entity.NextCost.Value = _config.ShippingConfiguration[_entity.Level.Value].Cost;
                    }
                    else
                    {
                        _viewModel.UpgradeAvailable.Value = false;
                    }
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

            _entity.UsedShippingRate.AsObservable().Subscribe(usedRate =>
            {
                var percentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    usedRate,
                    _entity.MaxShippingRate.Value
                );
                _viewModel.ShippingRatePercentage.Value = percentage;
            }).AddTo(MainDispatcher.Disposables);

            _entity.MaxShippingRate.AsObservable().Subscribe(maxCap =>
            {
                _shippingEntity.UpdateMaxShippingRate();
                _shippingEntity.UpdateUsedShippingRateOfAllVehicles();
                _viewModel.MaxShippingRate.Value = maxCap;
            }).AddTo(MainDispatcher.Disposables);
        }
        
        private void SubscribeToValueModifiers()
        {
            _valueModifiers.VehicleShippingRateIncrement
                .Subscribe(ApplyIncrementToVehicleShippingRate)
                .AddTo(MainDispatcher.Disposables);

            _valueModifiers.VehicleCostDiscount
                .Subscribe(ApplyDiscountToVehicle)
                .AddTo(MainDispatcher.Disposables);
        }
        
        private void ApplyValueModifiers()
        {
            ApplyIncrementToVehicleShippingRate(_valueModifiers.VehicleShippingRateIncrement.Value);
            ApplyDiscountToVehicle(_valueModifiers.VehicleCostDiscount.Value);
        }

        private void ApplyIncrementToVehicleShippingRate(float increment)
        {
            if (_entity.Purchased.Value)
            { 
                var baseValue = _config.ShippingConfiguration[_entity.Level.Value - 1].MaxShippingRate;
                var incrementValue = MathUtils.CalculateQuantityFromPercentage(
                    increment,
                    baseValue
                );
                _entity.MaxShippingRate.Value = baseValue + (int)incrementValue;
            }
        }
        
        private void ApplyDiscountToVehicle(float discount)
        {
            if (_entity.Level.Value < _config.ShippingConfiguration.Count)
            { 
                var baseValue = _config.ShippingConfiguration[_entity.Level.Value].Cost;
                var discountValue = MathUtils.CalculateQuantityFromPercentage(
                    discount,
                    baseValue
                );
                _entity.NextCost.Value = baseValue - discountValue;
            }
        }
    }
}