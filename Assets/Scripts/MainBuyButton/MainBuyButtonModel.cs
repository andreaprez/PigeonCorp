using PigeonCorp.Bonus;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonModel
    {
        public readonly int PigeonsPerClick;

        public MainBuyButtonModel(BonusModel bonusModel)
        {
            PigeonsPerClick = bonusModel.BuyButtonRate;
        }
    }
}