using System;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
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
        
        public BonusMediator(
            ResearchModel researchModel,
            BonusModel model,
            BonusView view,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand
        )
        {
            _researchModel = researchModel;
            _model = model;
            _view = view;
            _config = config;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            
            _view.GetResearchButtonAsObservable().Subscribe(research =>
            {
                var cost = _model.NextCost.Value;
                _model.Research();
                _subtractCurrencyCommand.Execute(cost);
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
    }
}