using PigeonCorp.MainScreen;
using PigeonCorp.UserState;

namespace PigeonCorp.Commands
{
    public class SpawnPigeonCommand : ICommand
    {
        private readonly UserStateModel _userStateModel;
        private readonly PigeonFactory _pigeonFactory;

        public SpawnPigeonCommand(UserStateModel userStateModel, PigeonFactory pigeonFactory)
        {
            _userStateModel = userStateModel;
            _pigeonFactory = pigeonFactory;
            
        }
        public void Handle()
        {
            _userStateModel.AddPigeons(1);
            _pigeonFactory.Create(1);
        }
    }
}