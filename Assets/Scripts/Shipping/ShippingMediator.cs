using System.Collections;
using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Hatchery;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class ShippingMediator
    {
        private readonly ShippingModel _model;
        private readonly ShippingView _view;
        private readonly ShippingTitleData _config;
        private readonly HatcheriesModel _hatcheriesModel;
        private readonly UserStateModel _userStateModel;
        private readonly ICommand<float> _subtractCurrencyCommand;
        private readonly List<VehicleBehaviour> _vehiclePrefabs;

        public ShippingMediator(
            ShippingModel model,
            ShippingView view,
            ShippingTitleData config,
            HatcheriesModel hatcheriesModel,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand,
            List<VehicleBehaviour> vehiclePrefabs
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _hatcheriesModel = hatcheriesModel;
            _userStateModel = userStateModel;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _vehiclePrefabs = vehiclePrefabs;

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
                var shippingRatePercentage = MathUtils.CalculatePercentage(
                    used,
                    _model.MaxShippingRate.Value
                );
                view.UpdateMaxShippingRateBar(shippingRatePercentage);
            }).AddTo(MainDispatcher.Disposables);
            
            model.MaxShippingRate.AsObservable().Subscribe(max =>
            {
                view.UpdateMaxShippingRateText(max);
                var shippingRatePercentage = MathUtils.CalculatePercentage(
                    _model.UsedShippingRate.Value,
                    max
                );
                view.UpdateMaxShippingRateBar(shippingRatePercentage);
            }).AddTo(MainDispatcher.Disposables);

            _hatcheriesModel.TotalProduction.AsObservable().Subscribe(production =>
            {
                _model.UpdateUsedShippingRate(production);
            }).AddTo(MainDispatcher.Disposables);

            MainThreadDispatcher.StartCoroutine(VehicleSpawner());

            InitializeSubViews();
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
                        _model.Vehicles[vehicleId].Purchase();
                        _subtractCurrencyCommand.Handle(cost);
                        Gateway.Instance.UpdateShippingData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);
                
                _view.GetVehicleView(vehicleId).GetUpgradeButtonAsObservable()
                    .Subscribe(upgrade =>
                    {
                        var cost = _model.Vehicles[vehicleId].NextCost.Value;
                        _model.Vehicles[vehicleId].Upgrade();
                        _subtractCurrencyCommand.Handle(cost);
                        Gateway.Instance.UpdateShippingData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);

                _model.Vehicles[i].Level.AsObservable().Subscribe(level =>
                    {
                        if (level > 0)
                        {
                            var prefabId = _model.Vehicles[vehicleId].Level.Value - 1;
                            var prefab = _vehiclePrefabs[prefabId];
                            _view.SetVehiclePrefab(vehicleId, prefab);
                        }

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
                        var percentage = MathUtils.CalculatePercentage(
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
            var percentageOfTotalShippingRate = MathUtils.CalculatePercentage(
                _model.Vehicles[vehicleId].MaxShippingRate.Value,
                _model.MaxShippingRate.Value
            );
            var usedQuantityFromPercentage =
                percentageOfTotalShippingRate * _model.UsedShippingRate.Value;

            _model.Vehicles[vehicleId].SetUsedShippingRate((int)usedQuantityFromPercentage);
        }

        private IEnumerator VehicleSpawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(_config.TimeToSpawnVehicleInSeconds);

                var randomId = Random.Range(0, _model.Vehicles.Count);
                
                while (!_model.Vehicles[randomId].Purchased.Value)
                {
                    randomId = Random.Range(0, _model.Vehicles.Count);
                }
                
                _view.SpawnVehicleInWorld(randomId, _config);
            }
        }
    }
}