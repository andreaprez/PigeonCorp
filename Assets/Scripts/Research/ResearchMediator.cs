using System;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using UniRx;

namespace PigeonCorp.Research
{
    public class ResearchMediator
    {
        private readonly ResearchModel _model;
        private readonly ResearchView _view;
        private readonly ResearchTitleData _config;
        private readonly ICommand<float> _subtractCurrencyCommand;
        
        public ResearchMediator(
            ResearchModel model,
            ResearchView view,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            
            view.GetOpenButtonAsObservable().Subscribe(open =>
            {
                view.Open();
            }).AddTo(MainDispatcher.Disposables);
            
            view.GetCloseButtonAsObservable().Subscribe(close =>
            {
                view.Close();
            }).AddTo(MainDispatcher.Disposables);
            
            InitializeSubViews();
        }
        
        private void InitializeSubViews()
        {
            for (int i = 0; i < _model.Bonuses.Count; i++)
            {
                var bonusId = i;
                
                _view.GetBonusView(bonusId).GetResearchButtonAsObservable()
                    .Subscribe(research =>
                    {
                        var cost = _model.Bonuses[bonusId].NextCost.Value;
                        _model.Bonuses[bonusId].Research();
                        _subtractCurrencyCommand.Handle(cost);
                        Gateway.Instance.UpdateResearchData(_model.Serialize());
                    }).AddTo(MainDispatcher.Disposables);

                _model.Bonuses[i].Tier.AsObservable().Subscribe(tier =>
                {
                    var bonusConfig = FindBonusConfigByType(_model.Bonuses[bonusId].Type);
                    var maxTier = bonusConfig.Tiers.Count;
                    if (tier == maxTier - 1)
                    {
                        _view.HideBonusResearchUI(bonusId);
                    }
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Bonuses[i].Name.AsObservable().Subscribe(name =>
                {
                    _view.SetBonusName(bonusId, name);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Bonuses[i].Icon.AsObservable().Subscribe(icon =>
                {
                    _view.SetBonusIcon(bonusId, icon);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Bonuses[i].NextCost.AsObservable().Subscribe(cost =>
                {
                    _view.SetBonusCost(bonusId, cost);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Bonuses[i].CurrentValue.AsObservable().Subscribe(currentValue =>
                {
                    _view.UpdateBonusCurrentValue(bonusId, currentValue);
                }).AddTo(MainDispatcher.Disposables);
                
                _model.Bonuses[i].NextValue.AsObservable().Subscribe(nextValue =>
                {
                    _view.UpdateBonusNextValue(bonusId, nextValue);
                }).AddTo(MainDispatcher.Disposables);
            }
        }
        
        private BonusConfig FindBonusConfigByType(BonusType type)
        {
            foreach (var bonus in _config.BonusTypesConfiguration)
            {
                if (bonus.Type == type)
                {
                    return bonus;
                }
            }

            throw new Exception("No config found for bonus type: " + type);
        }
    }
}