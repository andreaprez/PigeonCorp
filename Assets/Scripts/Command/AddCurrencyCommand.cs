using PigeonCorp.MainTopBar;

namespace PigeonCorp.Commands
{
    public class AddCurrencyCommand : ICommand<float>
    {
        private readonly MainTopBarEntity _mainTopBarEntity;

        public AddCurrencyCommand(MainTopBarEntity mainTopBarEntity)
        {
            _mainTopBarEntity = mainTopBarEntity;
        }
        
        public void Execute(float currency)
        {
            _mainTopBarEntity.AddCurrency(currency);
        }
    }
}