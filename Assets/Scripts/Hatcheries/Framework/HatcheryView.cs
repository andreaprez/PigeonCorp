using PigeonCorp.Dispatcher;
using PigeonCorp.Hatcheries.Adapter;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Hatcheries.Framework
{
    public class HatcheryView : MonoBehaviour
    {
        [SerializeField] private GameObject _emptyView;
        [SerializeField] private GameObject _builtView;
        [Space]
        [SerializeField] private Button _buildButton;
        [SerializeField] private Button _upgradeButton;
        [Space]
        [SerializeField] private int _id;
        [SerializeField] private Text _hatcheryName;
        [SerializeField] private Image _hatcheryIcon;
        [SerializeField] private Image _capacityBar;
        [SerializeField] private Text _capacityText;
        [SerializeField] private Text _buildCostText;
        [SerializeField] private Text _upgradeCostText;
        
        private HatcheryViewModel _viewModel;

        private void Start()
        {
            _viewModel = ProjectContext.Instance.Container
                .Resolve<HatcheriesViewModel>().HatcheryViewModels[_id];
            
            SubscribeToViewModel();
            SubscribeToButtons();
        }
        
        private void SubscribeToViewModel()
        {
            _viewModel.ButtonInteractable.Subscribe(interactable =>
            {
                _upgradeButton.interactable = interactable;
                _buildButton.interactable = interactable;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.UpgradeAvailable.Subscribe(available =>
            {
                _upgradeButton.gameObject.SetActive(available);
                _upgradeCostText.transform.parent.gameObject.SetActive(available);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Built.Subscribe(built =>
            {
                _emptyView.SetActive(!built);
                _builtView.SetActive(built);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Name.Subscribe(name =>
            {
                _hatcheryName.text = name;
                
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Icon.Subscribe(icon =>
            {
                _hatcheryIcon.sprite = icon;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.MaxCapacity.Subscribe(maxCapacity =>
            {
                _capacityText.text = maxCapacity.ToString();
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.CapacityPercentage.Subscribe(percentage =>
            {
                _capacityBar.fillAmount = percentage;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Cost.Subscribe(cost =>
            {
                _upgradeCostText.text = cost.ToString();
                _buildCostText.text = cost.ToString();
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToButtons()
        {
            _buildButton.OnClickAsObservable().Subscribe(u =>
            {
                ProjectContext.Instance.Container.Resolve<HatcheriesMediator>().OnBuildButtonClick(_id);
            }).AddTo(MainDispatcher.Disposables);

            _upgradeButton.OnClickAsObservable().Subscribe(u =>
            {
                ProjectContext.Instance.Container.Resolve<HatcheriesMediator>().OnUpgradeButtonClick(_id);
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}
