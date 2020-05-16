using System.Collections.Generic;
using UniRx;

namespace PigeonCorp.Research
{
    public class ResearchViewModel
    {
        public readonly ReactiveProperty<bool> IsOpen;
        public readonly List<BonusViewModel> BonusViewModels;

        public ResearchViewModel()
        {
            IsOpen = new ReactiveProperty<bool>();
            BonusViewModels = new List<BonusViewModel>();
        }
    }
}