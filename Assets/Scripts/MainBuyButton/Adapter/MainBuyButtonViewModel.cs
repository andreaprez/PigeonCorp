using UniRx;

namespace PigeonCorp.MainBuyButton.Adapter
{
    public class MainBuyButtonViewModel
    {
        public readonly ReactiveProperty<bool> IsInteractable;

        public MainBuyButtonViewModel()
        {
            IsInteractable = new ReactiveProperty<bool>();
        }
    }
}