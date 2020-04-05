using PigeonCorp.MainScreen;
using PigeonCorp.UserState;

namespace PigeonCorp.Commands
{
    public class BuyPigeonCommand : ICommand<int>
    {
        private readonly int _buyMultiplier;
        private readonly UserStateModel _userStateModel;
        private readonly PigeonFactory _pigeonFactory;

        public BuyPigeonCommand(
            int buyMultiplier,
            UserStateModel userStateModel,
            PigeonFactory pigeonFactory
        )
        {
            _buyMultiplier = buyMultiplier;
            _userStateModel = userStateModel;
            _pigeonFactory = pigeonFactory;
        }
        
        public void Handle(int modelPigeonsPerClick)
        {
            var pigeonsToAdd = modelPigeonsPerClick * _buyMultiplier;

            _userStateModel.AddPigeons(pigeonsToAdd);
            _pigeonFactory.Create(pigeonsToAdd);
        }
    }
}