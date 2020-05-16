using PigeonCorp.Dispatcher;
using PigeonCorp.MainBuyButton.Adapter;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.MainBuyButton.Framework
{
    public class MainBuyButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private MainBuyButtonViewModel _viewModel;

        public void Start()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<MainBuyButtonViewModel>();
            
            SubscribeToViewModel();
            SubscribeToButton();
        }
        
        private void SubscribeToViewModel()
        {
            _viewModel.IsInteractable.Subscribe(interactable =>
            {
                _button.interactable = interactable;
            }).AddTo(MainDispatcher.Disposables);
        }
        
        private void SubscribeToButton()
        {
            _button.OnClickAsObservable().Subscribe(u =>
            {
                ProjectContext.Instance.Container.Resolve<MainBuyButtonMediator>().OnButtonClick();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}