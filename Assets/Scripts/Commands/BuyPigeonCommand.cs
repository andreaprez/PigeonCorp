using PigeonCorp.MainBuyButton;
using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Commands
{
    public class BuyPigeonCommand : ICommand
    {
        private readonly MainBuyButtonModel _buyButtonModel;
        private readonly ICommand _spawnPigeonCommand;
        private readonly PigeonTitleData _pigeonConfig;
        private readonly ICommand<float> _subtractCurrencyCommand;

        public BuyPigeonCommand(
            MainBuyButtonModel buyButtonModel,
            ICommand spawnPigeonCommand,
            PigeonTitleData pigeonConfig,
            ICommand<float> subtractCurrencyCommand
        )
        {
            _buyButtonModel = buyButtonModel;
            _spawnPigeonCommand = spawnPigeonCommand;
            _pigeonConfig = pigeonConfig;
            _subtractCurrencyCommand = subtractCurrencyCommand;
        }
        
        public void Handle()
        {
            var pigeonsToAdd = _buyButtonModel.PigeonsPerClick;
            for (int i = 0; i < pigeonsToAdd; i++)
            {
                _spawnPigeonCommand.Handle();
            }

            var price = pigeonsToAdd * _pigeonConfig.Cost;
            _subtractCurrencyCommand.Handle(price);
        }
    }
}