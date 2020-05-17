using System.Collections;
using PigeonCorp.Command;
using PigeonCorp.Dispatcher;
using PigeonCorp.Hatcheries.Entity;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.Shipping.Entity;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers.Entity;
using PigeonCorp.ValueModifiers.UseCase;
using UniRx;
using UnityEngine;
using Zenject;

namespace PigeonCorp.Shipping.Adapter
{
    public class ShippingMediator
    {
        private readonly ShippingViewModel _viewModel;

        private ShippingEntity _entity;
        private ShippingTitleData _config;
        private HatcheriesEntity _hatcheriesEntity;
        private MainTopBarEntity _mainTopBarEntity;
        private ICommand<float> _subtractCurrencyCommand;
        private ICommand<int> _spawnVehicleCommand;
        private ICommand _grantShippingRevenueCommand;
        private ShippingValueModifiers _valueModifiers;

        public ShippingMediator()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<ShippingViewModel>();
        }

        public void Initialize(
            ShippingEntity entity,
            ShippingTitleData config,
            HatcheriesEntity hatcheriesEntity,
            MainTopBarEntity mainTopBarEntity,
            ICommand<float> subtractCurrencyCommand,
            ICommand<int> spawnVehicleCommand,
            ICommand grantShippingRevenueCommand,
            UC_GetShippingValueModifiers getShippingValueModifiersUC
        )
        {
            _entity = entity;
            _config = config;
            _hatcheriesEntity = hatcheriesEntity;
            _mainTopBarEntity = mainTopBarEntity;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _spawnVehicleCommand = spawnVehicleCommand;
            _grantShippingRevenueCommand = grantShippingRevenueCommand;
            _valueModifiers = (ShippingValueModifiers) getShippingValueModifiersUC.Execute();

            InitializeSubViewModels();
            InitializeSubMediators();
            SubscribeToEntity();
            SubscribeToTotalProduction();
            SubscribeToCurrency();
            
            MainThreadDispatcher.StartCoroutine(VehicleSpawner());
            MainThreadDispatcher.StartCoroutine(GrantShippingRevenue());
        }
        
        public void OnOpenButtonClick()
        {
            _viewModel.IsOpen.Value = true;
        }

        public void OnCloseButtonClick()
        {
            _viewModel.IsOpen.Value = false;
        }

        public void OnPurchaseButtonClick(int id)
        {
            var cost = _entity.Vehicles[id].NextCost.Value;
            _subtractCurrencyCommand.Execute(cost);
                    
            _entity.Vehicles[id].Purchase();
            
            Gateway.Instance.UpdateShippingData(SerializeEntityModel());
        }

        public void OnUpgradeButtonClick(int id)
        {
            var cost = _entity.Vehicles[id].NextCost.Value;
            _subtractCurrencyCommand.Execute(cost);
                    
            _entity.Vehicles[id].Upgrade();
            
            Gateway.Instance.UpdateShippingData(SerializeEntityModel());
        }

        private void InitializeSubViewModels()
        {
            for (int i = 0; i < _entity.Vehicles.Count; i++)
            {
                var viewModel = new VehicleViewModel();
                _viewModel.VehicleViewModels.Add(viewModel);
            }
        }
        
        private void InitializeSubMediators()
        {
            for (int i = 0; i < _entity.Vehicles.Count; i++)
            {
                new VehicleMediator().Initialize(
                    _entity,
                    _entity.Vehicles[i],
                    _config,
                    _valueModifiers
                );
            }
        }
        
        private void SubscribeToEntity()
        {
            _entity.UsedShippingRate.AsObservable().Subscribe(used =>
            {
                _entity.UpdateUsedShippingRateOfAllVehicles();
                var shippingRatePercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    used,
                    _entity.MaxShippingRate.Value
                );
                _viewModel.ShippingRatePercentage.Value = shippingRatePercentage;
            }).AddTo(MainDispatcher.Disposables);

            _entity.MaxShippingRate.AsObservable().Subscribe(maxRate =>
            {
                _entity.UpdateUsedShippingRate();
                _viewModel.MaxShippingRate.Value = maxRate;
                var shippingRatePercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    _entity.UsedShippingRate.Value,
                    maxRate
                );
                _viewModel.ShippingRatePercentage.Value = shippingRatePercentage;
            }).AddTo(MainDispatcher.Disposables);
        }
        
        private void SubscribeToTotalProduction()
        {
            _hatcheriesEntity.TotalProduction.AsObservable().Subscribe(production =>
            {
                _entity.UpdateUsedShippingRate();
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToCurrency()
        {
            _mainTopBarEntity.Currency.AsObservable().Subscribe(currency =>
            {
                foreach (var vehicle in _entity.Vehicles)
                {
                    var nextCost = vehicle.NextCost.Value;
                    var enoughCurrency = currency >= nextCost;
                    _viewModel.VehicleViewModels[vehicle.Id].ButtonInteractable.Value = enoughCurrency;
                }
            }).AddTo(MainDispatcher.Disposables);
        }

        private IEnumerator VehicleSpawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(_config.TimeToSpawnVehicleInSeconds);

                if (_entity.UsedShippingRate.Value > 0)
                {
                    var randomId = Random.Range(0, _entity.Vehicles.Count);

                    while (!_entity.Vehicles[randomId].Purchased.Value)
                    {
                        randomId = Random.Range(0, _entity.Vehicles.Count);
                    }

                    var prefabId = _entity.Vehicles[randomId].Level.Value - 1;
                    _spawnVehicleCommand.Execute(prefabId);
                }
            }
        }
        
        private IEnumerator GrantShippingRevenue()
        {
            while (true)
            {
                yield return new WaitForSeconds(60);
                
                _grantShippingRevenueCommand.Execute();
            }
        }
        
        private ShippingUserData SerializeEntityModel()
        {
            return new ShippingUserData(_entity);
        }
    }
}