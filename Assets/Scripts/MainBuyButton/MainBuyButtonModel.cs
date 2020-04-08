using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonModel
    {
        public readonly int PigeonsPerClick;

        public MainBuyButtonModel(MainBuyButtonTitleData config)
        {
            PigeonsPerClick = config.PigeonsPerClick;
        }
    }
}