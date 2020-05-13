using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonModel
    {
        public int PigeonsPerClick;
        public float PigeonCost;

        public MainBuyButtonModel(PigeonTitleData pigeonConfig)
        {
            PigeonsPerClick = 1;
            PigeonCost = pigeonConfig.Cost;
        }
    }
}