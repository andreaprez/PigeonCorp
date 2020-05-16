using PigeonCorp.Dispatcher;
using PigeonCorp.MainTopBar.Adapter;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.MainTopBar.Framework
{
    public class MainTopBarView : MonoBehaviour
    {
        [SerializeField] private Text _currency;
        [SerializeField] private Text _pigeonsCount;

        private MainTopBarViewModel _viewModel;

        public void Start()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<MainTopBarViewModel>();
            
            SubscribeToViewModel();
        }

        private void SubscribeToViewModel()
        {
            _viewModel.Currency.Subscribe(currency =>
            {
                _currency.text = currency.ToString();
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.PigeonsCount.Subscribe(pigeons =>
            {
                _pigeonsCount.text = pigeons.ToString();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}