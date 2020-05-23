using PigeonCorp.Dispatcher;
using PigeonCorp.Evolution.Adapter;
using PigeonCorp.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Evolution.Framework
{
    public class EvolutionView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Button _nextEggButton;
        [SerializeField] private Button _previousEggButton;
        [Space]
        [SerializeField] private Button _evolveButton;
        [Space]
        [SerializeField] private Image _openButtonIcon;
        [SerializeField] private Image _pigeonIcon;
        [SerializeField] private Text _pigeonNameText;
        [SerializeField] private Text _pigeonValueText;
        [SerializeField] private Text _currentFarmValueText;
        [SerializeField] private Text _requiredFarmValueText;
        
        private EvolutionViewModel _viewModel;

        private void Awake()
        {
            _viewModel = ProjectContext.Instance.Container.Resolve<EvolutionViewModel>();
            
            SubscribeToViewModel();
            SubscribeToButtons();
        }

        private void SubscribeToViewModel()
        {
            _viewModel.IsOpen.Subscribe(isOpen =>
            {
                gameObject.SetActive(isOpen);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.OpenButtonIcon.Subscribe(icon =>
            {
                _openButtonIcon.sprite = icon;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.PigeonIcon.Subscribe(icon =>
            {
                _pigeonIcon.sprite = icon;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.PigeonName.Subscribe(name =>
            {
                _pigeonNameText.text = name;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.EggValue.Subscribe(pigeonValue =>
            {
                _pigeonValueText.text = DisplayableNumber.Parse(DisplayableNumber.THOUSAND_SEPARATOR, pigeonValue);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.CurrentFarmValue.Subscribe(value =>
            {
                _currentFarmValueText.text = DisplayableNumber.Parse(DisplayableNumber.THOUSAND_SEPARATOR_WITH_ONE_DECIMAL, value);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.RequiredFarmValue.Subscribe(required =>
            {
                _requiredFarmValueText.text = DisplayableNumber.Parse(DisplayableNumber.THOUSAND_SEPARATOR_WITH_ONE_DECIMAL, required);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.ButtonInteractable.Subscribe(interactable =>
            {
                _evolveButton.interactable = interactable;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.EvolutionAvailable.Subscribe(available =>
            {
                _evolveButton.gameObject.SetActive(available);
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToButtons()
        {
            _openButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<EvolutionMediator>().OnOpenButtonClick();
            }).AddTo(MainDispatcher.Disposables);

            _closeButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<EvolutionMediator>().OnCloseButtonClick();
            }).AddTo(MainDispatcher.Disposables);
            
            _nextEggButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<EvolutionMediator>().OnNextEggButtonClick();
            }).AddTo(MainDispatcher.Disposables);

            _previousEggButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<EvolutionMediator>().OnPreviousEggButtonClick();
            }).AddTo(MainDispatcher.Disposables);
            
            _evolveButton.OnClickAsObservable().Subscribe(onClick =>
            {
                ProjectContext.Instance.Container.Resolve<EvolutionMediator>().OnEvolveButtonClick();
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}
