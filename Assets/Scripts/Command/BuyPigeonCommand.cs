using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Commands
{
    public class BuyPigeonCommand : ICommand<int>
    {
        private readonly ICommand _spawnPigeonCommand;
        private readonly PigeonTitleData _pigeonConfig;
        private readonly ICommand<float> _subtractCurrencyCommand;

        public BuyPigeonCommand(
            ICommand spawnPigeonCommand,
            PigeonTitleData pigeonConfig,
            ICommand<float> subtractCurrencyCommand
        )
        {
            _spawnPigeonCommand = spawnPigeonCommand;
            _pigeonConfig = pigeonConfig;
            _subtractCurrencyCommand = subtractCurrencyCommand;
        }
        
        public void Execute(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                _spawnPigeonCommand.Execute();
            }

            var price = quantity * _pigeonConfig.Cost;
            _subtractCurrencyCommand.Execute(price);
        }
    }
}