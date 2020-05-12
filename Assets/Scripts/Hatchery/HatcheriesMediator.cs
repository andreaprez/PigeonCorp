using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research;
using PigeonCorp.UserState;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Hatcheries
{
    public class HatcheriesMediator
    {
        private readonly HatcheriesModel _model;
        private readonly HatcheriesView _view;
        private readonly HatcheriesTitleData _config;
        private readonly UserStateModel _userStateModel;
        private readonly ICommand<float> _subtractCurrencyCommand;
        private readonly ICommand<int, int> _spawnHatcheryCommand;
        private readonly HatcheriesValueModifiers _valueModifiers;


        public HatcheriesMediator(
            HatcheriesModel model,
            HatcheriesView view,
            HatcheriesTitleData config,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand,
            ICommand<int, int> spawnHatcheryCommand,
            List<Transform> hatcheryEntrances,
            UC_GetHatcheriesValueModifiers getHatcheriesValueModifiersUc
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _userStateModel = userStateModel;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _spawnHatcheryCommand = spawnHatcheryCommand;
            
            _valueModifiers = (HatcheriesValueModifiers)getHatcheriesValueModifiersUc.Execute();

            model.SetHatcheryEntrances(hatcheryEntrances);
            
            view.GetOpenButtonAsObservable().Subscribe(open =>
            {
                view.Open();
            }).AddTo(MainDispatcher.Disposables);
            
            view.GetCloseButtonAsObservable().Subscribe(close =>
            {
                view.Close();
            }).AddTo(MainDispatcher.Disposables);
            
            model.UsedCapacity.AsObservable().Subscribe(used =>
            {
                var capacityPercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    used,
                    _model.MaxCapacity.Value
                );
                view.UpdateMaxCapacityBar(capacityPercentage);
            }).AddTo(MainDispatcher.Disposables);
            
            model.MaxCapacity.AsObservable().Subscribe(max =>
            {
                model.UpdateUsedCapacity();
                view.UpdateMaxCapacityText(max);
                var capacityPercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    _model.UsedCapacity.Value,
                    max
                );
                view.UpdateMaxCapacityBar(capacityPercentage);
            }).AddTo(MainDispatcher.Disposables);
            
            userStateModel.CurrentPigeons.AsObservable().Subscribe(pigeons =>
            {
                model.UpdateUsedCapacity();
            })
            .AddTo(MainDispatcher.Disposables);

            InitializeSubViews();

            SubscribeToBonusValues();
        }

        private void InitializeSubViews()
        {
            for (int i = 0; i < _model.Hatcheries.Count; i++)
            {
                var hatcheryId = i;
                
                _model.Hatcheries[i].Built.AsObservable().Subscribe(built =>
                {
                    _view.SetHatcheryBuilt(hatcheryId, built);
                }).AddTo(MainDispatcher.Disposables);
                
                _view.GetHatcheryView(hatcheryId).GetBuildButtonAsObservable()
                    .Subscribe(build =>
                    {
                        var cost = _model.Hatcheries[hatcheryId].NextCost.Value;
                        _model.Hatcheries[hatcheryId].Build();
                        _subtractCurrencyCommand.Execute(cost);
                        ApplyMultiplierToEggLayingRate(hatcheryId, _valueModifiers.EggLayingRateMultiplier.Value);
                        ApplyIncrementToMaxCapacity(hatcheryId, _valueModifiers.HatcheryCapacityIncrement.Value);
                        Gateway.Instance.UpdateHatcheriesData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);
                
                _view.GetHatcheryView(hatcheryId).GetUpgradeButtonAsObservable()
                    .Subscribe(upgrade =>
                    {
                        var cost = _model.Hatcheries[hatcheryId].NextCost.Value;
                        _model.Hatcheries[hatcheryId].Upgrade();
                        _subtractCurrencyCommand.Execute(cost);
                        ApplyMultiplierToEggLayingRate(hatcheryId, _valueModifiers.EggLayingRateMultiplier.Value);
                        ApplyIncrementToMaxCapacity(hatcheryId, _valueModifiers.HatcheryCapacityIncrement.Value);
                        Gateway.Instance.UpdateHatcheriesData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);

                _model.Hatcheries[i].Level.AsObservable().Subscribe(level =>
                {
                    if (level > 0)
                    {
                        var prefabId = _model.Hatcheries[hatcheryId].Level.Value - 1;
                        _spawnHatcheryCommand.Execute(prefabId, hatcheryId);
                    }

                    var maxLevel = _config.HatcheriesConfiguration.Count;
                    if (level == maxLevel)
                    {
                        _view.HideHatcheryUpgradeUI(hatcheryId);
                    }
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Hatcheries[i].Name.AsObservable().Subscribe(name =>
                {
                    _view.SetHatcheryName(hatcheryId, name);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Hatcheries[i].Icon.AsObservable().Subscribe(icon =>
                {
                    _view.SetHatcheryIcon(hatcheryId, icon);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Hatcheries[i].NextCost.AsObservable().Subscribe(cost =>
                {
                    _view.SetHatcheryCost(hatcheryId, cost);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Hatcheries[i].UsedCapacity.AsObservable().Subscribe(usedCap =>
                {
                    var percentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                        usedCap,
                        _model.Hatcheries[hatcheryId].MaxCapacity.Value
                    );
                    _view.UpdateHatcheryCapacityPercentage(hatcheryId, percentage);
                    
                    _model.UpdateTotalProduction();
                }).AddTo(MainDispatcher.Disposables);

                _model.Hatcheries[i].MaxCapacity.AsObservable().Subscribe(maxCap =>
                {
                    _model.UpdateMaxCapacity();
                    _view.UpdateHatcheryMaxCapacity(hatcheryId, maxCap);
                    for (int id = 0; id < _model.Hatcheries.Count; id++)
                    {
                        SetUsedCapacityOfSingleHatchery(id);
                    }
                }).AddTo(MainDispatcher.Disposables);

                _model.Hatcheries[i].EggLayingRate.AsObservable().Subscribe(layingRate =>
                {
                    _model.UpdateTotalProduction();
                }).AddTo(MainDispatcher.Disposables);
                
                _model.UsedCapacity.AsObservable().Subscribe(used =>
                {
                    SetUsedCapacityOfSingleHatchery(hatcheryId);
                }).AddTo(MainDispatcher.Disposables);
                
                _userStateModel.Currency.AsObservable().Subscribe(currency => 
                {
                    var nextCost = _model.Hatcheries[hatcheryId].NextCost.Value;
                    var enoughCurrency = currency >= nextCost;
                    _view.SetButtonInteractable(hatcheryId, enoughCurrency);
                }).AddTo(MainDispatcher.Disposables);
            }
        }
        
        private void SetUsedCapacityOfSingleHatchery(int hatcheryId)
        {
            var percentageOfTotalCapacity = MathUtils.CalculatePercentageDecimalFromQuantity(
                _model.Hatcheries[hatcheryId].MaxCapacity.Value,
                _model.MaxCapacity.Value
            );
            var usedQuantityFromPercentage =
                percentageOfTotalCapacity * _model.UsedCapacity.Value;

            _model.Hatcheries[hatcheryId].SetUsedCapacity((int)usedQuantityFromPercentage);
        }
        
        private void SubscribeToBonusValues()
        {
            for (int i = 0; i < _model.Hatcheries.Count; i++)
            {
                var hatcheryId = i;
                
                _valueModifiers.EggLayingRateMultiplier.Subscribe(multiplier =>
                {
                    ApplyMultiplierToEggLayingRate(hatcheryId, multiplier);
                }).AddTo(MainDispatcher.Disposables);

                _valueModifiers.HatcheryCapacityIncrement.Subscribe(increment =>
                {
                    ApplyIncrementToMaxCapacity(hatcheryId, increment);
                }).AddTo(MainDispatcher.Disposables);
            }
        }

        private void ApplyMultiplierToEggLayingRate(int hatcheryId, float multiplier)
        {
            var hatchery = _model.Hatcheries[hatcheryId];
            if (hatchery.Built.Value)
            { 
                var baseValue = _config.HatcheriesConfiguration[hatchery.Level.Value - 1].EggLayingRate;
                hatchery.EggLayingRate.Value = baseValue * multiplier;
            }
        }
        
        private void ApplyIncrementToMaxCapacity(int hatcheryId, float increment)
        {
            var hatchery = _model.Hatcheries[hatcheryId];
            if (hatchery.Built.Value)
            { 
                var baseValue = _config.HatcheriesConfiguration[hatchery.Level.Value - 1].MaxCapacity;
                var incrementValue = MathUtils.CalculateQuantityFromPercentage(
                    increment,
                    hatchery.MaxCapacity.Value
                );
                hatchery.MaxCapacity.Value = baseValue + (int)incrementValue;
            }
        }
    }
}