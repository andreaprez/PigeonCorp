using System;
using PigeonCorp.Dispatcher;
using PigeonCorp.Hatcheries.Adapter;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Hatcheries.Framework
{
    public class HatcheriesView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Text _maxCapacityText;
        [SerializeField] private Image _maxCapacityBar;
        
        private HatcheriesViewModel _viewModel;

        private void Awake()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<HatcheriesViewModel>();
            
            SubscribeToViewModel();
            SubscribeToButtons();
        }

        private void SubscribeToViewModel()
        {
            _viewModel.MaxCapacity.Subscribe(maxCapacity =>
            {
                _maxCapacityText.text = String.Format(DisplayableNumberFormat.THOUSAND_SEPARATOR, maxCapacity);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.CapacityPercentage.Subscribe(percentage =>
            {
                _maxCapacityBar.fillAmount = percentage;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.IsOpen.Subscribe(isOpen =>
            {
                gameObject.SetActive(isOpen);
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToButtons()
        {
            _openButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<HatcheriesMediator>().OnOpenButtonClick();
            }).AddTo(MainDispatcher.Disposables);

            _closeButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<HatcheriesMediator>().OnCloseButtonClick();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}