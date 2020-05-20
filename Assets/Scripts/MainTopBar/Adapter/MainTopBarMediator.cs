using System.Collections;
using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;
using UniRx;
using UnityEngine;
using Zenject;

namespace PigeonCorp.MainTopBar.Adapter
{
    public class MainTopBarMediator
    {
        private readonly MainTopBarViewModel _viewModel;
     
        private MainTopBarEntity _entity;

        public MainTopBarMediator()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<MainTopBarViewModel>();
        }
        
        public void Initialize(MainTopBarEntity entity)
        {
            _entity = entity;

            SubscribeToEntity();
            
            MainThreadDispatcher.StartCoroutine(SaveLastTimeOnline());
        }

        private void SubscribeToEntity()
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

        private IEnumerator SaveLastTimeOnline()
        {
            while (true)
            {
                Gateway.Instance.UpdateUserStateData(SerializeEntityModel());
                yield return new WaitForSeconds(10);
            }
        }
        
        private UserStateUserData SerializeEntityModel()
        {
            return new UserStateUserData(_entity);
        }
    }
}