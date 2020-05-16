using PigeonCorp.Command;
using PigeonCorp.MainTopBar.Entity;

namespace PigeonCorp.MainTopBar.UseCase
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