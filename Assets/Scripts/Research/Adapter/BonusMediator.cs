using System;
using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research.Entity;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers.Entity;
using UniRx;
using Zenject;

namespace PigeonCorp.Research.Adapter
{
    public class BonusMediator
    {
        private BonusEntity _entity;
        private BonusConfig _config;
        private ResearchValueModifiers _valueModifiers;
        private BonusViewModel _viewModel;

        public void Initialize(
            BonusEntity entity,
            BonusConfig config,
            ResearchValueModifiers valueModifiers
        )
        {
            _entity = entity;
            _config = config;
            _valueModifiers = valueModifiers;

            _viewModel = ProjectContext.Instance.Container
                .Resolve<ResearchViewModel>().BonusViewModels[_entity.Id];
            _viewModel.ButtonInteractable.Value = true;
            _viewModel.ResearchAvailable.Value = true;

            SubscribeToEntity();
            SubscribeToValueModifiers();
        }

        private void SubscribeToEntity()
        {
            _entity.Tier.AsObservable().Subscribe(level =>
            {
                if (level > 0)
                {
                    _entity.Name.Value = _config.Name;
                    _entity.CurrentValue.Value = _config.Tiers[_entity.Tier.Value].Value;
                    if (_entity.Tier.Value < _config.Tiers.Count - 1)
                    {
                        _entity.NextValue.Value = _config.Tiers[_entity.Tier.Value + 1].Value;
                        _entity.NextCost.Value = _config.Tiers[_entity.Tier.Value + 1].Cost;
                    }
                    else
                    {
                        _viewModel.ResearchAvailable.Value = false;
                    }
                }
                ApplyValueModifiers();
            }).AddTo(MainDispatcher.Disposables);

            _entity.Name.AsObservable().Subscribe(name =>
            {
                _viewModel.Name.Value = name;
            }).AddTo(MainDispatcher.Disposables);

            _entity.NextCost.AsObservable().Subscribe(cost =>
            {
                _viewModel.Cost.Value = cost;
            }).AddTo(MainDispatcher.Disposables);

            _entity.CurrentValue.AsObservable().Subscribe(current =>
            {
                _entity.ApplyBonus();
                var value = GetValueFormatWithType(current);
                _viewModel.CurrentValue.Value = value;
            }).AddTo(MainDispatcher.Disposables);

            _entity.NextValue.AsObservable().Subscribe(next =>
            {
                var value = GetValueFormatWithType(next);
                _viewModel.NextValue.Value = value;
            }).AddTo(MainDispatcher.Disposables);
        }

        private String GetValueFormatWithType(float value)
        {
            switch (_entity.UnitType.Position)
            {
                case UnitPosition.START:
                    return _entity.UnitType.Unit + value;
                case UnitPosition.END:
                    return value + _entity.UnitType.Unit;
                default:
                    return value.ToString();
            }
        }
        
        private void SubscribeToValueModifiers()
        {
            _valueModifiers.ResearchCostDiscount
                .Subscribe(ApplyDiscountToBonus)
                .AddTo(MainDispatcher.Disposables);
        }

        private void ApplyValueModifiers()
        {
            ApplyDiscountToBonus(_valueModifiers.ResearchCostDiscount.Value);
        }

        private void ApplyDiscountToBonus(float discount)
        {
            var maxTier = _config.Tiers.Count - 1;
            if (_entity.Tier.Value < maxTier)
            {
                var baseValue = _config.Tiers[_entity.Tier.Value + 1].Cost;
                var discountValue = MathUtils.CalculateQuantityFromPercentage(
                    discount,
                    baseValue
                );
                _entity.NextCost.Value = baseValue - discountValue;
            }
        }
    }
}