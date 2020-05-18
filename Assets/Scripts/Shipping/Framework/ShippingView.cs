using System;
using PigeonCorp.Dispatcher;
using PigeonCorp.Shipping.Adapter;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Shipping.Framework
{
    public class ShippingView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Text _maxShippingRateText;
        [SerializeField] private Image _maxShippingRateBar;

        private ShippingViewModel _viewModel;

        private void Awake()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<ShippingViewModel>();
            
            SubscribeToViewModel();
            SubscribeToButtons();
        }

        private void SubscribeToViewModel()
        {
            _viewModel.MaxShippingRate.Subscribe(maxRate =>
            {
                _maxShippingRateText.text = String.Format(DisplayableNumberFormat.THOUSAND_SEPARATOR, maxRate);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.ShippingRatePercentage.Subscribe(percentage =>
            {
                _maxShippingRateBar.fillAmount = percentage;
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
                ProjectContext.Instance.Container.Resolve<ShippingMediator>().OnOpenButtonClick();
            }).AddTo(MainDispatcher.Disposables);

            _closeButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<ShippingMediator>().OnCloseButtonClick();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}