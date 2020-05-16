using System.Collections.Generic;
using UniRx;

namespace PigeonCorp.Hatcheries.Adapter
{
    public class HatcheriesViewModel
    {
        public readonly ReactiveProperty<bool> IsOpen;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<float> CapacityPercentage;
        public readonly List<HatcheryViewModel> HatcheryViewModels;

        public HatcheriesViewModel()
        {
            IsOpen = new ReactiveProperty<bool>();
            MaxCapacity = new ReactiveProperty<int>();
            CapacityPercentage = new ReactiveProperty<float>();
            HatcheryViewModels = new List<HatcheryViewModel>();
        }
    }
}