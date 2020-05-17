using PigeonCorp.Command;
using PigeonCorp.Dispatcher;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.ValueModifiers.Entity;
using PigeonCorp.ValueModifiers.UseCase;
using UniRx;
using Zenject;

namespace PigeonCorp.Evolution.Adapter
{
    public class EvolutionMediator
    {
        private readonly EvolutionViewModel _viewModel;

        private EvolutionEntity _entity;
        private EvolutionTitleData _config;
        private ICommand _resetFarmCommand;
        private EvolutionValueModifiers _valueModifiers;
        private int _lastSelectedId;
        
        public EvolutionMediator()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<EvolutionViewModel>();
        }

        public void Initialize(
            EvolutionEntity entity,
            EvolutionTitleData config,
            ICommand resetFarmCommand,
            UC_GetEvolutionValueModifiers getEvolutionValueModifiersUC
        )
        {
            _entity = entity;
            _config = config;
            _resetFarmCommand = resetFarmCommand;
            _valueModifiers = (EvolutionValueModifiers)getEvolutionValueModifiersUC.Execute();

            _lastSelectedId = _entity.SelectedEggId.Value;
            
            _viewModel.ButtonInteractable.Value = true;
            _viewModel.EvolutionAvailable.Value = true;

            InitializeSubViewModels();
            SubscribeToEntity();
            SubscribeToSubEntities();
            SubscribeToValueModifiers();
        }
        
        public void OnOpenButtonClick()
        {
            _entity.SelectedEggId.Value = _entity.CurrentEggId.Value;
            _viewModel.IsOpen.Value = true;
        }

        public void OnCloseButtonClick()
        {
            _viewModel.IsOpen.Value = false;
        }

        public void OnNextEggButtonClick()
        {
            var currentIsLastEgg = _entity.SelectedEggId.Value == _config.EvolutionsConfiguration.Count - 1;
            if (!currentIsLastEgg)
            {
                var nextEggData = _entity.EvolutionEggs[_entity.SelectedEggId.Value + 1];
                var nextEggIsDiscovered = nextEggData.IsDiscovered.Value;
                if (nextEggIsDiscovered)
                {
                    _entity.SelectedEggId.Value += 1;
                }
            }
        }

        public void OnPreviousEggButtonClick()
        {
            if (_entity.SelectedEggId.Value > 0)
            {
                _entity.SelectedEggId.Value -= 1;
            }
        }
        
        public void OnEvolveButtonClick()
        {
            _entity.Evolve();
            Gateway.Instance.UpdateEvolutionData(SerializeEntityModel());
            
            _resetFarmCommand.Execute();
        }

        private void InitializeSubViewModels()
        {
            for (int i = 0; i < _entity.EvolutionEggs.Count; i++)
            {
                var viewModel = new EvolutionEggViewModel();
                _viewModel.EvolutionEggViewModels.Add(viewModel);
            }
        }

        private void SubscribeToEntity()
        {
            _entity.CurrentEggId.AsObservable().Subscribe(id =>
            {
                _entity.EvolutionEggs[id].IsDiscovered.Value = true;
                _entity.SelectedEggId.Value = id;

                var currentEggConfig = _config.EvolutionsConfiguration[id];
                _entity.CurrentPigeonIcon.Value = currentEggConfig.Icon;
                _entity.CurrentPigeonName.Value = currentEggConfig.Name;
                _entity.CurrentEggValue.Value = currentEggConfig.EggValue * _valueModifiers.EggValueMultiplier.Value;
                
                if (id < _config.EvolutionsConfiguration.Count - 1)
                {
                    var nextEvolutionConfig = _config.EvolutionsConfiguration[id + 1];
                    _entity.RequiredFarmValue.Value = nextEvolutionConfig.RequiredFarmValue;
                }
                else
                {
                    _viewModel.EvolutionAvailable.Value = false;
                }

                ApplyValueModifiers();
            }).AddTo(MainDispatcher.Disposables);
            
            _entity.SelectedEggId.AsObservable().Subscribe(id =>
            {
                var selectedEggConfig = _config.EvolutionsConfiguration[id];
                _viewModel.PigeonIcon.Value = selectedEggConfig.Icon;
                _viewModel.PigeonName.Value = selectedEggConfig.Name;
                _viewModel.EggValue.Value = selectedEggConfig.EggValue * _valueModifiers.EggValueMultiplier.Value;
                
                _viewModel.EvolutionEggViewModels[_lastSelectedId].IsSelected.Value = false;
                _viewModel.EvolutionEggViewModels[id].IsSelected.Value = true;
                _lastSelectedId = id;
            }).AddTo(MainDispatcher.Disposables);

            _entity.CurrentPigeonIcon.AsObservable().Subscribe(icon =>
            {
                _viewModel.PigeonIcon.Value = icon;
            }).AddTo(MainDispatcher.Disposables);
            
            _entity.CurrentPigeonName.AsObservable().Subscribe(name =>
            {
                _viewModel.PigeonName.Value = name;
            }).AddTo(MainDispatcher.Disposables);
            
            _entity.CurrentEggValue.AsObservable().Subscribe(value =>
            {
                _viewModel.EggValue.Value = value;
            }).AddTo(MainDispatcher.Disposables);
            
            _entity.CurrentFarmValue.AsObservable().Subscribe(farmValue =>
            {
                _viewModel.CurrentFarmValue.Value = farmValue;
                
                var canEvolve = farmValue >= _entity.RequiredFarmValue.Value;
                _viewModel.ButtonInteractable.Value = canEvolve;
                
                Gateway.Instance.UpdateEvolutionData(SerializeEntityModel());
            }).AddTo(MainDispatcher.Disposables);

            _entity.RequiredFarmValue.AsObservable().Subscribe(requiredFarmValue =>
            {
                _viewModel.RequiredFarmValue.Value = requiredFarmValue;
                
                var canEvolve = _entity.CurrentFarmValue.Value >= requiredFarmValue;
                _viewModel.ButtonInteractable.Value = canEvolve;
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToSubEntities()
        {
            for (int i = 0; i < _entity.EvolutionEggs.Count; i++)
            {
                var eggId = i;
                _entity.EvolutionEggs[eggId].IsDiscovered.AsObservable().Subscribe(discovered =>
                {
                    _viewModel.EvolutionEggViewModels[eggId].IsDiscovered.Value = discovered;
                }).AddTo(MainDispatcher.Disposables);
            }
        }

        private void SubscribeToValueModifiers()
        {
            _valueModifiers.EggValueMultiplier
                .Subscribe(ApplyMultiplierToEggValue)
                .AddTo(MainDispatcher.Disposables);
        }
        
        private void ApplyValueModifiers()
        {
            ApplyMultiplierToEggValue(_valueModifiers.EggValueMultiplier.Value);
        }
        
        private void ApplyMultiplierToEggValue(float multiplier)
        {
            var baseValue = _config.EvolutionsConfiguration[_entity.CurrentEggId.Value].EggValue;
            _entity.CurrentEggValue.Value = baseValue * multiplier;
        }

        private EvolutionUserData SerializeEntityModel()
        {
            return new EvolutionUserData(_entity);
        }
    }
}