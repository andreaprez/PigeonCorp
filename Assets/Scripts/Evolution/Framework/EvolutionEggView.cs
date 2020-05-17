using PigeonCorp.Dispatcher;
using PigeonCorp.Evolution.Adapter;
using UniRx;
using UnityEngine;
using Zenject;

namespace PigeonCorp.Evolution.Framework
{
    public class EvolutionEggView : MonoBehaviour
    {
        [SerializeField] private RectTransform _discoveredView;
        [SerializeField] private RectTransform _hiddenView;
        [Space]
        [SerializeField] private int _id;
        
        private EvolutionEggViewModel _viewModel;

        private void Start()
        {
            _viewModel = ProjectContext.Instance.Container
                .Resolve<EvolutionViewModel>().EvolutionEggViewModels[_id];
            
            SubscribeToViewModel();
        }
        
        private void SubscribeToViewModel()
        {
            _viewModel.IsDiscovered.Subscribe(discovered =>
            {
                _discoveredView.gameObject.SetActive(discovered);
                _hiddenView.gameObject.SetActive(!discovered);
            }).AddTo(MainDispatcher.Disposables);
            
            _viewModel.IsSelected.Subscribe(selected =>
            {
                if (selected)
                {
                    _discoveredView.localScale = new Vector3(1.6f, 1.6f, 1f);
                    _hiddenView.localScale = new Vector3(1.6f, 1.6f, 1f);
                }
                else
                {
                    _discoveredView.localScale = Vector3.one;
                    _hiddenView.localScale = Vector3.one;
                }
            }).AddTo(MainDispatcher.Disposables);
        }
    }
}