using PigeonCorp.Dispatcher;
using PigeonCorp.Installers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarView : MonoBehaviour
    {
        [SerializeField] private Text _currency;
        [SerializeField] private Text _pigeonsCount;
        
        private MainTopBarViewModel _viewModel;

        
        public void Start()
        {
            _viewModel = Containers.Scene.Resolve<MainTopBarViewModel>();
            
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