using UniRx;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarViewModel
    {
        public readonly ReactiveProperty<int> PigeonsCount;
        public readonly ReactiveProperty<float> Currency;
        
        public MainTopBarViewModel()
        {
            PigeonsCount = new ReactiveProperty<int>();
            Currency = new ReactiveProperty<float>();
        }
    }
}