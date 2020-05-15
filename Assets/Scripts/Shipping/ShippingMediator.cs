using System.Collections;
using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Hatcheries;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class ShippingMediator
    {
        private readonly ShippingModel _model;
        private readonly ShippingView _view;
        private readonly ShippingTitleData _config;
        private readonly HatcheriesEntity _hatcheriesEntity;
        private readonly UserStateModel _userStateModel;
        private readonly ICommand<float> _subtractCurrencyCommand;
        private readonly ICommand<int> _spawnVehicleCommand;
        private readonly ICommand _grantShippingRevenueCommand;
        private readonly ShippingValueModifiers _valueModifiers;

        public ShippingMediator(
            ShippingModel model,
            ShippingView view,
            ShippingTitleData config,
            HatcheriesEntity hatcheriesEntity,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand,
            ICommand<int> spawnVehicleCommand,
            ICommand grantShippingRevenueCommand,
            UC_GetShippingValueModifiers getShippingValueModifiersUC
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _hatcheriesEntity = hatcheriesEntity;
            _userStateModel = userStateModel;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _spawnVehicleCommand = spawnVehicleCommand;
            _grantShippingRevenueCommand = grantShippingRevenueCommand;

            _valueModifiers = (ShippingValueModifiers)getShippingValueModifiersUC.Execute();

            view.GetOpenButtonAsObservable().Subscribe(open =>
            {
                view.Open();
            }).AddTo(MainDispatcher.Disposables);
            
            view.GetCloseButtonAsObservable().Subscribe(close =>
            {
                view.Close();
            }).AddTo(MainDispatcher.Disposables);
            
            model.UsedShippingRate.AsObservable().Subscribe(used =>
            {
                var shippingRatePercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    used,
                    _model.MaxShippingRate.Value
                );
                view.UpdateMaxShippingRateBar(shippingRatePercentage);
            }).AddTo(MainDispatcher.Disposables);
            
            model.MaxShippingRate.AsObservable().Subscribe(max =>
            {
                model.UpdateUsedShippingRate();
                view.UpdateMaxShippingRateText(max);
                var shippingRatePercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    _model.UsedShippingRate.Value,
                    max
                );
                view.UpdateMaxShippingRateBar(shippingRatePercentage);
            }).AddTo(MainDispatcher.Disposables);

            _hatcheriesEntity.TotalProduction.AsObservable().Subscribe(production =>
            {
                _model.UpdateUsedShippingRate();
            }).AddTo(MainDispatcher.Disposables);

            InitializeSubViews();

            SubscribeToValueModifiers();
           
            MainThreadDispatcher.StartCoroutine(VehicleSpawner());
            MainThreadDispatcher.StartCoroutine(GrantShippingRevenue());
        }

        private void InitializeSubViews()
        {
            for (int i = 0; i < _model.Vehicles.Count; i++)
            {
                var vehicleId = i;
                
                _model.Vehicles[i].Purchased.AsObservable().Subscribe(purchased =>
                    {
                        _view.SetVehiclePurchased(vehicleId, purchased);
                    }).AddTo(MainDispatcher.Disposables);
                
                _view.GetVehicleView(vehicleId).GetPurchaseButtonAsObservable()
                    .Subscribe(purchase =>
                    {
                        var cost = _model.Vehicles[vehicleId].NextCost.Value;
                        _subtractCurrencyCommand.Execute(cost);
                        
                        _model.Vehicles[vehicleId].Purchase();
                        ApplyValueModifiers(vehicleId);

                        Gateway.Instance.UpdateShippingData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);
                
                _view.GetVehicleView(vehicleId).GetUpgradeButtonAsObservable()
                    .Subscribe(upgrade =>
                    {
                        var cost = _model.Vehicles[vehicleId].NextCost.Value;
                        _subtractCurrencyCommand.Execute(cost);
                        
                        _model.Vehicles[vehicleId].Upgrade();
                        ApplyValueModifiers(vehicleId);

                        Gateway.Instance.UpdateShippingData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);

                _model.Vehicles[i].Level.AsObservable().Subscribe(level =>
                    {
                        var maxLevel = _config.ShippingConfiguration.Count;
                        if (level == maxLevel)
                        {
                            _view.HideVehicleUpgradeUI(vehicleId);
                        }
                    }).AddTo(MainDispatcher.Disposables);
                
                _model.Vehicles[i].Name.AsObservable().Subscribe(name =>
                    {
                        _view.SetVehicleName(vehicleId, name);
                    }).AddTo(MainDispatcher.Disposables);
                
                _model.Vehicles[i].Icon.AsObservable().Subscribe(icon =>
                    {
                        _view.SetVehicleIcon(vehicleId, icon);
                    }).AddTo(MainDispatcher.Disposables);
                
                _model.Vehicles[i].NextCost.AsObservable().Subscribe(cost =>
                    {
                        _view.SetVehicleCost(vehicleId, cost);
                    }).AddTo(MainDispatcher.Disposables);
                
                _model.Vehicles[i].UsedShippingRate.AsObservable().Subscribe(usedRate =>
                    {
                        var percentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                            usedRate,
                            _model.Vehicles[vehicleId].MaxShippingRate.Value
                        );
                        _view.UpdateVehicleShippingRatePercentage(vehicleId, percentage);
                    }).AddTo(MainDispatcher.Disposables);
                
                _model.Vehicles[i].MaxShippingRate.AsObservable().Subscribe(maxRate =>
                    {
                        _model.UpdateMaxShippingRate();
                        _view.UpdateVehicleMaxShippingRate(vehicleId, maxRate);
                        for (int id = 0; id < _model.Vehicles.Count; id++)
                        {
                            SetUsedShippingRateOfSingleVehicle(id);
                        }
                    }).AddTo(MainDispatcher.Disposables);

                _model.UsedShippingRate.AsObservable().Subscribe(used =>
                    {
                        SetUsedShippingRateOfSingleVehicle(vehicleId);
                    }).AddTo(MainDispatcher.Disposables);
                
                _userStateModel.Currency.AsObservable().Subscribe(currency => 
                    {
                        var nextCost = _model.Vehicles[vehicleId].NextCost.Value;
                        var enoughCurrency = currency >= nextCost;
                        _view.SetButtonInteractable(vehicleId, enoughCurrency);
                    })
                    .AddTo(MainDispatcher.Disposables);
            }
        }

        private void SetUsedShippingRateOfSingleVehicle(int vehicleId)
        {
            var percentageOfTotalShippingRate = MathUtils.CalculatePercentageDecimalFromQuantity(
                _model.Vehicles[vehicleId].MaxShippingRate.Value,
                _model.MaxShippingRate.Value
            );
            var usedQuantityFromPercentage =
                percentageOfTotalShippingRate * _model.UsedShippingRate.Value;

            _model.Vehicles[vehicleId].SetUsedShippingRate((int)usedQuantityFromPercentage);
        }
        
        private void SubscribeToValueModifiers()
        {
            for (int i = 0; i < _model.Vehicles.Count; i++)
            {
                var vehicleId = i;
                
                _valueModifiers.VehicleShippingRateIncrement.Subscribe(increment =>
                {
                    ApplyIncrementToVehicleShippingRate(vehicleId, increment);
                }).AddTo(MainDispatcher.Disposables);

                _valueModifiers.VehicleCostDiscount.Subscribe(discount =>
                {
                    ApplyDiscountToVehicle(vehicleId, discount);
                }).AddTo(MainDispatcher.Disposables);
            }
        }
        
        private void ApplyValueModifiers(int vehicleId)
        {
            ApplyIncrementToVehicleShippingRate(vehicleId, _valueModifiers.VehicleShippingRateIncrement.Value);
            ApplyDiscountToVehicle(vehicleId, _valueModifiers.VehicleCostDiscount.Value);
        }

        private void ApplyIncrementToVehicleShippingRate(int vehicleId, float increment)
        {
            var vehicle = _model.Vehicles[vehicleId];
            if (vehicle.Purchased.Value)
            { 
                var baseValue = _config.ShippingConfiguration[vehicle.Level.Value - 1].MaxShippingRate;
                var incrementValue = MathUtils.CalculateQuantityFromPercentage(
                    increment,
                    baseValue
                );
                vehicle.MaxShippingRate.Value = baseValue + (int)incrementValue;
            }
        }
        
        private void ApplyDiscountToVehicle(int vehicleId, float discount)
        {
            var vehicle = _model.Vehicles[vehicleId];
            if (vehicle.Level.Value < _config.ShippingConfiguration.Count)
            { 
                var baseValue = _config.ShippingConfiguration[vehicle.Level.Value].Cost;
                var discountValue = MathUtils.CalculateQuantityFromPercentage(
                    discount,
                    baseValue
                );
                vehicle.NextCost.Value = baseValue - discountValue;
            }
        }

        private IEnumerator VehicleSpawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(_config.TimeToSpawnVehicleInSeconds);

                if (_model.MaxShippingRate.Value > 0)
                {
                    var randomId = Random.Range(0, _model.Vehicles.Count);

                    while (!_model.Vehicles[randomId].Purchased.Value)
                    {
                        randomId = Random.Range(0, _model.Vehicles.Count);
                    }

                    var prefabId = _model.Vehicles[randomId].Level.Value - 1;
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
    }
}