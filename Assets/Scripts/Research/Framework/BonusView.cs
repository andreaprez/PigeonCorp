using PigeonCorp.Dispatcher;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PigeonCorp.Research
{
    public class BonusView : MonoBehaviour
    {
        [SerializeField] private Button _researchButton;
        [Space]
        [SerializeField] private int _id;
        [SerializeField] private Text _bonusName;
        [SerializeField] private Image _bonusIcon;
        [SerializeField] private Text _researchCostText;
        [SerializeField] private Text _currentValueText;
        [SerializeField] private Text _nextValueText;
        [SerializeField] private Image _nextValueArrow;
        
        private BonusViewModel _viewModel;

        private void Start()
        {
            _viewModel = ProjectContext.Instance.Container
                .Resolve<ResearchViewModel>().BonusViewModels[_id];
            
            SubscribeToViewModel();
            
            SubscribeToButtons();
        }
        
        private void SubscribeToViewModel()
        {
            _viewModel.ButtonInteractable.Subscribe(interactable =>
            {
                _researchButton.interactable = interactable;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.ResearchAvailable.Subscribe(available =>
            {
                _researchButton.gameObject.SetActive(available);
                _researchCostText.transform.parent.gameObject.SetActive(available);
                _nextValueText.gameObject.SetActive(available);
                _nextValueArrow.gameObject.SetActive(available);            
            }).AddTo(MainDispatcher.Disposables);

            _viewModel.Name.Subscribe(name =>
            {
                _bonusName.text = name;
                
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Icon.Subscribe(icon =>
            {
                _bonusIcon.sprite = icon;
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.CurrentValue.Subscribe(value =>
            {
                _currentValueText.text = value.ToString();
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.NextValue.Subscribe(value =>
            {
                _nextValueText.text = value.ToString();
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.Cost.Subscribe(cost =>
            {
                _researchCostText.text = cost.ToString();
            }).AddTo(MainDispatcher.Disposables);
        }

        private void SubscribeToButtons()
        {
            _researchButton.OnClickAsObservable().Subscribe(u =>
            {
                ProjectContext.Instance.Container.Resolve<ResearchMediator>().OnResearchButtonClick(_id);
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}