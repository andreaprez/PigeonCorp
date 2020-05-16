using UniRx;

namespace PigeonCorp.MainTopBar.Entity
{
    public class MainTopBarEntity
    {
        public readonly ReactiveProperty<int> PigeonsCount;
        public readonly ReactiveProperty<float> Currency;

        public MainTopBarEntity()
        {
            PigeonsCount = new ReactiveProperty<int>();
            Currency = new ReactiveProperty<float>();
        }

        public void AddPigeons(int pigeonsToAdd)
        {
            PigeonsCount.Value += pigeonsToAdd;
        }

        public void SubtractCurrency(float price)
        {
            Currency.Value -= price;
        }
        
        public void AddCurrency(float quantity)
        {
            Currency.Value += quantity;
        }
    }
}