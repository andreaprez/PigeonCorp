using PigeonCorp.Dispatcher;
using PigeonCorp.Shipping.Adapter;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Shipping.Framework
{
    public class VehicleView : MonoBehaviour
    {
        [SerializeField] private GameObject _emptyView;
        [SerializeField] private GameObject _obtainedView;
        [Space]
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _upgradeButton;
        [Space]
        [SerializeField] private int _id;
        [SerializeField] private Text _vehicleName;
        [SerializeField] private Image _vehicleIcon;
        [SerializeField] private Image _shippingRateBar;
        [SerializeField] private Text _shippingRateText;
        [SerializeField] private Text _purchaseCostText;
        [SerializeField] private Text _upgradeCostText;

        private VehicleViewModel _viewModel;

        private void Start()
        {
            _viewModel = ProjectContext.Instance.Container
                .Resolve<ShippingViewModel>().VehicleViewModels[_id];
            
            SubscribeToViewModel();
            SubscribeToButtons();
        }
        
        private void SubscribeToViewModel()
        {
            _viewModel.ButtonInteractable.Subscribe(interactable =>
            {
                _upgradeButton.interactable = interactable;
                _purchaseButton.interactable = interactable;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.UpgradeAvailable.Subscribe(available =>
            {
                _upgradeButton.gameObject.SetActive(available);
                _upgradeCostText.transform.parent.gameObject.SetActive(available);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Purchased.Subscribe(purchased =>
            {
                _emptyView.SetActive(!purchased);
                _obtainedView.SetActive(purchased);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Name.Subscribe(name =>
            {
                _vehicleName.text = name;
                
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Icon.Subscribe(icon =>
            {
                _vehicleIcon.sprite = icon;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.MaxShippingRate.Subscribe(maxRate =>
            {
                _shippingRateText.text = DisplayableNumber.Parse(DisplayableNumber.THOUSAND_SEPARATOR, maxRate);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.ShippingRatePercentage.Subscribe(percentage =>
            {
                _shippingRateBar.fillAmount = percentage;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Cost.Subscribe(cost =>
            {
                _upgradeCostText.text = DisplayableNumber.Parse(DisplayableNumber.THOUSAND_SEPARATOR, cost);
                _purchaseCostText.text = DisplayableNumber.Parse(DisplayableNumber.THOUSAND_SEPARATOR, cost);
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToButtons()
        {
            _purchaseButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<ShippingMediator>().OnPurchaseButtonClick(_id);
            }).AddTo(MainDispatcher.Disposables);

            _upgradeButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<ShippingMediator>().OnUpgradeButtonClick(_id);
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}