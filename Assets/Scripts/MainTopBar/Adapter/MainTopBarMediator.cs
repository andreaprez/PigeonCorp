using PigeonCorp.Dispatcher;
using PigeonCorp.Installers;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;
using UniRx;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarMediator
    {
        private readonly MainTopBarEntity _entity;
        private readonly MainTopBarViewModel _viewModel;

        public MainTopBarMediator(MainTopBarEntity entity)
        {
            _entity = entity;
            _viewModel = Containers.Scene.Resolve<MainTopBarViewModel>();
        }
        
        public void Initialize()
        {
            _entity.Currency.AsObservable().Subscribe(currency =>
            {
                Gateway.Instance.UpdateUserStateData(SerializeEntityModel());
                _viewModel.Currency.Value = currency;
            })
            .AddTo(MainDispatcher.Disposables);
            
            _entity.PigeonsCount.AsObservable().Subscribe(pigeons =>
            {
                Gateway.Instance.UpdateUserStateData(SerializeEntityModel());
                _viewModel.PigeonsCount.Value = pigeons;
            })
            .AddTo(MainDispatcher.Disposables);
        }
        
        private UserStateUserData SerializeEntityModel()
        {
            return new UserStateUserData(_entity);
        }
    }
}