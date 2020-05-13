using System;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;

namespace PigeonCorp.Research
{
    public class BonusMediator
    {
        private readonly ResearchModel _researchModel;
        private readonly BonusModel _model;
        private readonly BonusView _view;
        private readonly ResearchTitleData _config;
        private readonly ICommand<float> _subtractCurrencyCommand;
        private readonly ResearchValueModifiers _valueModifiers;

        public BonusMediator(
            ResearchModel researchModel,
            BonusModel model,
            BonusView view,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand,
            ResearchValueModifiers valueModifiers
        )
        {
            _researchModel = researchModel;
            _model = model;
            _view = view;
            _config = config;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _valueModifiers = valueModifiers;

            _view.GetResearchButtonAsObservable().Subscribe(research =>
            {
                var cost = _model.NextCost.Value;
                _subtractCurrencyCommand.Execute(cost);
                
                _model.Research();
                ApplyValueModifiers();
                
                Gateway.Instance.UpdateResearchData(_researchModel.Serialize());
            }).AddTo(MainDispatcher.Disposables);

            _model.Tier.AsObservable().Subscribe(tier =>
            {
                var bonusConfig = FindBonusConfigByType();
                var maxTier = bonusConfig.Tiers.Count;
                if (tier == maxTier - 1)
                {
                    _view.HideResearchUI();
                }
            }).AddTo(MainDispatcher.Disposables);
            
            _model.Name.AsObservable().Subscribe(name =>
            {
                _view.SetName(name);
            }).AddTo(MainDispatcher.Disposables);
            
            _model.Icon.AsObservable().Subscribe(icon =>
            {
                _view.SetIcon(icon);
            }).AddTo(MainDispatcher.Disposables);
            
            _model.NextCost.AsObservable().Subscribe(cost =>
            {
                _view.SetCost(cost);
            }).AddTo(MainDispatcher.Disposables);
            
            _model.CurrentValue.AsObservable().Subscribe(currentValue =>
            {
                _model.ApplyBonus();
                _view.UpdateCurrentValue(currentValue);
            }).AddTo(MainDispatcher.Disposables);
            
            _model.NextValue.AsObservable().Subscribe(nextValue =>
            {
                _view.UpdateNextValue(nextValue);
            }).AddTo(MainDispatcher.Disposables);

            SubscribeToValueModifiers();
        }

        private BonusConfig FindBonusConfigByType()
        {
            foreach (var bonus in _config.BonusTypesConfiguration)
            {
                if (bonus.Type == _model.Type)
                {
                    return bonus;
                }
            }

            throw new Exception("No config found for bonus type: " + _model.Type);
        }
        
        private void SubscribeToValueModifiers()
        {
            _valueModifiers.ResearchCostDiscount.Subscribe(discount =>
            {
                ApplyDiscountToBonus(discount);
            }).AddTo(MainDispatcher.Disposables);
        }
        
        private void ApplyValueModifiers()
        {
            ApplyDiscountToBonus(_valueModifiers.ResearchCostDiscount.Value);
        }

        private void ApplyDiscountToBonus(float discount)
        {
            var bonusConfig = FindBonusConfigByType();
            var maxTier = bonusConfig.Tiers.Count - 1;
            if (_model.Tier.Value < maxTier)
            {
                var baseValue = bonusConfig.Tiers[_model.Tier.Value + 1].Cost;
                var discountValue = MathUtils.CalculateQuantityFromPercentage(
                    discount,
                    baseValue
                );
                _model.NextCost.Value = baseValue - discountValue;
            }
        }
    }
}