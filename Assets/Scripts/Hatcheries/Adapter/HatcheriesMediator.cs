using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.Utils;
using PigeonCorp.ValueModifiers;
using UniRx;
using Zenject;

namespace PigeonCorp.Hatcheries
{
    public class HatcheriesMediator
    {
        private readonly HatcheriesViewModel _viewModel;

        private HatcheriesEntity _entity;
        private HatcheriesTitleData _config;
        private MainTopBarEntity _mainTopBarEntity;
        private ICommand<float> _subtractCurrencyCommand;
        private ICommand<int, int> _spawnHatcheryCommand;
        private HatcheriesValueModifiers _valueModifiers;
        
        public HatcheriesMediator()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<HatcheriesViewModel>();
        }

        public void Initialize(
            HatcheriesEntity entity,
            HatcheriesTitleData config,
            MainTopBarEntity mainTopBarEntity,
            ICommand<float> subtractCurrencyCommand,
            ICommand<int, int> spawnHatcheryCommand,
            UC_GetHatcheriesValueModifiers getHatcheriesValueModifiersUC
        )
        {
            _entity = entity;
            _config = config;
            _mainTopBarEntity = mainTopBarEntity;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _spawnHatcheryCommand = spawnHatcheryCommand;
            _valueModifiers = (HatcheriesValueModifiers)getHatcheriesValueModifiersUC.Execute();

            SubscribeToPigeonsCount();
            SubscribeToEntity();
            InitializeSubViewModels();
            InitializeSubMediators();
        }

        public void OnOpenButtonClick()
        {
            _viewModel.IsOpen.Value = true;
        }

        public void OnCloseButtonClick()
        {
            _viewModel.IsOpen.Value = false;
        }

        public void OnBuildButtonClick(int id)
        {
            var cost = _entity.Hatcheries[id].NextCost.Value;
            _subtractCurrencyCommand.Execute(cost);
                    
            _entity.Hatcheries[id].Build();
            
            Gateway.Instance.UpdateHatcheriesData(SerializeEntityModel());
        }

        public void OnUpgradeButtonClick(int id)
        {
            var cost = _entity.Hatcheries[id].NextCost.Value;
            _subtractCurrencyCommand.Execute(cost);
                    
            _entity.Hatcheries[id].Upgrade();
            
            Gateway.Instance.UpdateHatcheriesData(SerializeEntityModel());
        }
        
        private void SubscribeToPigeonsCount()
        {
            _mainTopBarEntity.PigeonsCount.AsObservable().Subscribe(pigeons =>
            {
                _entity.UpdateUsedCapacity();
            })
            .AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToEntity()
        {
            _entity.UsedCapacity.AsObservable().Subscribe(used =>
            {
                _entity.UpdateUsedCapacityOfAllHatcheries();
                var capacityPercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    used,
                    _entity.MaxCapacity.Value
                );
                _viewModel.CapacityPercentage.Value = capacityPercentage;
            }).AddTo(MainDispatcher.Disposables);

            _entity.MaxCapacity.AsObservable().Subscribe(maxCap =>
            {
                _entity.UpdateUsedCapacity();
                _viewModel.MaxCapacity.Value = maxCap;
                var capacityPercentage = MathUtils.CalculatePercentageDecimalFromQuantity(
                    _entity.UsedCapacity.Value,
                    maxCap
                );
                _viewModel.CapacityPercentage.Value = capacityPercentage;
            }).AddTo(MainDispatcher.Disposables);
        }

        private void InitializeSubViewModels()
        {
            for (int i = 0; i < _entity.Hatcheries.Count; i++)
            {
                var viewModel = new HatcheryViewModel();
                _viewModel.HatcheryViewModels.Add(viewModel);
            }
        }
        
        private void InitializeSubMediators()
        {
            for (int i = 0; i < _entity.Hatcheries.Count; i++)
            {
                new HatcheryMediator().Initialize(
                    _entity,
                    _entity.Hatcheries[i],
                    _config,
                    _mainTopBarEntity,
                    _spawnHatcheryCommand,
                    _valueModifiers
                );
            }
        }
        
        private HatcheriesUserData SerializeEntityModel()
        {
            return new HatcheriesUserData(_entity);
        }
    }
}
