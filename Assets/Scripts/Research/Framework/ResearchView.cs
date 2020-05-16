using PigeonCorp.Dispatcher;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Research
{
    public class ResearchView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        
        private ResearchViewModel _viewModel;

        private void Awake()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<ResearchViewModel>();
            
            SubscribeToViewModel();
            
            SubscribeToButtons();
        }

        private void SubscribeToViewModel()
        {
            _viewModel.IsOpen.Subscribe(isOpen =>
            {
                gameObject.SetActive(isOpen);
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToButtons()
        {
            _openButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<ResearchMediator>().OnOpenButtonClick();
            }).AddTo(MainDispatcher.Disposables);

            _closeButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<ResearchMediator>().OnCloseButtonClick();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}