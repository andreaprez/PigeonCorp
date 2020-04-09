using PigeonCorp.UserState;
using UniRx;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarModel
    {
        public readonly ReactiveProperty<int> PigeonsCount;
        public readonly ReactiveProperty<float> Currency;

        public MainTopBarModel(UserStateModel userState)
        {
            PigeonsCount = new ReactiveProperty<int>(userState.CurrentPigeons.Value);
            Currency = new ReactiveProperty<float>(userState.Currency.Value);
        }
        
        public void SetPigeonsCount(int count)
        {
            PigeonsCount.Value = count;
        }

        public void SetCurrency(float currency)
        {
            Currency.Value = currency;
        }
    }
}