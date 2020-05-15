using UniRx;

namespace PigeonCorp.MainBuyButton
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