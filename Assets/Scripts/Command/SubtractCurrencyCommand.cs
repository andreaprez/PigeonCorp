using PigeonCorp.MainTopBar;

namespace PigeonCorp.Commands
{
    public class SubtractCurrencyCommand : ICommand<float>
    {
        private readonly MainTopBarEntity _mainTopBarEntity;

        public SubtractCurrencyCommand(MainTopBarEntity mainTopBarEntity)
        {
            _mainTopBarEntity = mainTopBarEntity;
        }
        
        public void Execute(float currency)
        {
            _mainTopBarEntity.SubtractCurrency(currency);
        }
    }
}