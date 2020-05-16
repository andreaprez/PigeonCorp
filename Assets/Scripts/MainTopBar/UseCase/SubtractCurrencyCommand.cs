using PigeonCorp.Command;
using PigeonCorp.MainTopBar.Entity;

namespace PigeonCorp.MainTopBar.UseCase
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