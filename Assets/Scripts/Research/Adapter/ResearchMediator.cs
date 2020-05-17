using System;
using PigeonCorp.Command;
using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.Research.Entity;
using PigeonCorp.ValueModifiers.Entity;
using PigeonCorp.ValueModifiers.UseCase;
using UniRx;
using Zenject;

namespace PigeonCorp.Research.Adapter
{
    public class ResearchMediator
    {
        private readonly ResearchViewModel _viewModel;

        private ResearchEntity _entity;
        private ResearchTitleData _config;
        private MainTopBarEntity _mainTopBarEntity;
        private ICommand<float> _subtractCurrencyCommand;
        private ResearchValueModifiers _valueModifiers;
        
        public ResearchMediator()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<ResearchViewModel>();
        }
        
        public void Initialize(
            ResearchEntity entity,
            ResearchTitleData config,
            MainTopBarEntity mainTopBarEntity,
            ICommand<float> subtractCurrencyCommand,
            UC_GetResearchValueModifiers getResearchValueModifiersUC
        )
        {
            _entity = entity;
            _config = config;
            _mainTopBarEntity = mainTopBarEntity;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _valueModifiers = (ResearchValueModifiers)getResearchValueModifiersUC.Execute();
            
            InitializeSubViewModels();
            InitializeSubMediators();
            SubscribeToCurrency();
        }
        
        public void OnOpenButtonClick()
        {
            _viewModel.IsOpen.Value = true;
        }

        public void OnCloseButtonClick()
        {
            _viewModel.IsOpen.Value = false;
        }

        public void OnResearchButtonClick(int id)
        {
            var cost = _entity.Bonuses[id].NextCost.Value;
            _subtractCurrencyCommand.Execute(cost);
                    
            _entity.Bonuses[id].Research();
            
            Gateway.Instance.UpdateResearchData(SerializeEntityModel());
        }
        
        private void InitializeSubViewModels()
        {
            for (int i = 0; i < _entity.Bonuses.Count; i++)
            {
                var viewModel = new BonusViewModel();
                _viewModel.BonusViewModels.Add(viewModel);
            }
        }
        
        private void InitializeSubMediators()
        {
            for (int i = 0; i < _entity.Bonuses.Count; i++)
            {
                var bonusEntity = _entity.Bonuses[i];
                var bonusConfig = FindBonusConfigByType(bonusEntity.Type);
                
                new BonusMediator().Initialize(
                    bonusEntity,
                    bonusConfig,
                    _valueModifiers
                );
            }
        }

        private void SubscribeToCurrency()
        {
            _mainTopBarEntity.Currency.AsObservable().Subscribe(currency =>
            {
                foreach (var bonus in _entity.Bonuses)
                {
                    var nextCost = bonus.NextCost.Value;
                    var enoughCurrency = currency >= nextCost;
                    _viewModel.BonusViewModels[bonus.Id].ButtonInteractable.Value = enoughCurrency;
                }
            }).AddTo(MainDispatcher.Disposables);
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

        private ResearchUserData SerializeEntityModel()
        {
            return new ResearchUserData(_entity);
        }
    }
}