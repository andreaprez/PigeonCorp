using PigeonCorp.MainBuyButton;
using PigeonCorp.MainScreen;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;

namespace PigeonCorp.Commands
{
    public class BuyPigeonCommand : ICommand
    {
        private readonly MainBuyButtonModel _buyButtonModel;
        private readonly UserStateModel _userStateModel;
        private readonly PigeonFactory _pigeonFactory;
        private readonly PigeonTitleData _pigeonConfig;
        private readonly int _buyMultiplier;

        public BuyPigeonCommand(
            MainBuyButtonModel buyButtonModel,
            UserStateModel userStateModel,
            PigeonFactory pigeonFactory,
            PigeonTitleData pigeonConfig,
            int buyMultiplier
        )
        {
            _buyButtonModel = buyButtonModel;
            _userStateModel = userStateModel;
            _pigeonFactory = pigeonFactory;
            _pigeonConfig = pigeonConfig;
            _buyMultiplier = buyMultiplier;
        }
        
        public void Handle()
        {
            var pigeonsToAdd = _buyButtonModel.PigeonsPerClick * _buyMultiplier;
            _userStateModel.AddPigeons(pigeonsToAdd);
            _pigeonFactory.Create(pigeonsToAdd);

            var price = pigeonsToAdd * _pigeonConfig.Cost;
            _userStateModel.SubtractCurrency(price);
        }
    }
}