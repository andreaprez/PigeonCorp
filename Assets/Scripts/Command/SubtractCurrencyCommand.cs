using PigeonCorp.UserState;

namespace PigeonCorp.Commands
{
    public class SubtractCurrencyCommand : ICommand<float>
    {
        private readonly UserStateModel _userStateModel;

        public SubtractCurrencyCommand(UserStateModel userStateModel)
        {
            _userStateModel = userStateModel;
        }
        
        public void Execute(float currency)
        {
            _userStateModel.SubtractCurrency(currency);
        }
    }
}