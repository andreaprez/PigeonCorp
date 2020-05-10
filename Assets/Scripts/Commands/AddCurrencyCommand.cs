using PigeonCorp.UserState;

namespace PigeonCorp.Commands
{
    public class AddCurrencyCommand : ICommand<float>
    {
        private readonly UserStateModel _userStateModel;

        public AddCurrencyCommand(UserStateModel userStateModel)
        {
            _userStateModel = userStateModel;
        }
        
        public void Handle(float currency)
        {
            _userStateModel.AddCurrency(currency);
        }
    }
}