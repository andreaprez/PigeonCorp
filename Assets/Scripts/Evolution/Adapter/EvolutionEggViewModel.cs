using UniRx;

namespace PigeonCorp.Evolution.Adapter
{
    public class EvolutionEggViewModel
    {
        public readonly ReactiveProperty<bool> IsDiscovered;
        public readonly ReactiveProperty<bool> IsSelected;

        public EvolutionEggViewModel()
        {
            IsDiscovered = new ReactiveProperty<bool>();
            IsSelected = new ReactiveProperty<bool>();
        }
    }
}