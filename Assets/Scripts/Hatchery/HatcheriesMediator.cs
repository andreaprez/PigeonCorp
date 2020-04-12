using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Hatchery
{
    public class HatcheriesMediator
    {
        private readonly HatcheriesModel _model;
        private readonly HatcheriesView _view;
        private readonly HatcheriesTitleData _config;
        private readonly UserStateModel _userStateModel;
        private readonly ICommand<float> _subtractCurrencyCommand;
        private readonly List<GameObject> _hatcheryPrefabs;

        public HatcheriesMediator(
            HatcheriesModel model,
            HatcheriesView view,
            HatcheriesTitleData config,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand,
            List<GameObject> hatcheryPrefabs
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _userStateModel = userStateModel;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _hatcheryPrefabs = hatcheryPrefabs;

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
                var capacityPercentage = MathUtils.CalculatePercentage(
                    used,
                    _model.MaxCapacity.Value
                );
                view.UpdateMaxCapacityBar(capacityPercentage);
            }).AddTo(MainDispatcher.Disposables);
            
            model.MaxCapacity.AsObservable().Subscribe(max =>
            {
                view.UpdateMaxCapacityText(max);
                var capacityPercentage = MathUtils.CalculatePercentage(
                    _model.UsedCapacity.Value,
                    max
                );
                view.UpdateMaxCapacityBar(capacityPercentage);
            }).AddTo(MainDispatcher.Disposables);
            
            userStateModel.CurrentPigeons.AsObservable()
                .Subscribe(model.UpdateUsedCapacity)
                .AddTo(MainDispatcher.Disposables);

            InitializeSubViews();
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
                        _subtractCurrencyCommand.Handle(cost);
                        Gateway.Instance.UpdateHatcheriesData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);
                
                _view.GetHatcheryView(hatcheryId).GetUpgradeButtonAsObservable()
                    .Subscribe(upgrade =>
                    {
                        var cost = _model.Hatcheries[hatcheryId].NextCost.Value;
                        _model.Hatcheries[hatcheryId].Upgrade();
                        _subtractCurrencyCommand.Handle(cost);
                        Gateway.Instance.UpdateHatcheriesData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);

                _model.Hatcheries[i].Level.AsObservable().Subscribe(level =>
                    {
                        if (level > 0)
                        {
                            var prefabId = _model.Hatcheries[hatcheryId].Level.Value - 1;
                            var prefab = _hatcheryPrefabs[prefabId];
                            _view.PlaceHatcheryBuildingInWorld(hatcheryId, prefab);
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
                        var percentage = MathUtils.CalculatePercentage(
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
                    })
                    .AddTo(MainDispatcher.Disposables);
            }
        }

        private void SetUsedCapacityOfSingleHatchery(int hatcheryId)
        {
            var percentageOfTotalCapacity = MathUtils.CalculatePercentage(
                _model.Hatcheries[hatcheryId].MaxCapacity.Value,
                _model.MaxCapacity.Value
            );
            var usedQuantityFromPercentage =
                percentageOfTotalCapacity * _model.UsedCapacity.Value;

            _model.Hatcheries[hatcheryId].SetUsedCapacity((int)usedQuantityFromPercentage);
        }
    }
}