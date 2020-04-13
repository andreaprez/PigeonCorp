using PigeonCorp.Factory;
using PigeonCorp.UserState;

namespace PigeonCorp.Commands
{
    public class SpawnPigeonCommand : ICommand
    {
        private readonly UserStateModel _userStateModel;
        private readonly IFactory<int> _pigeonFactory;

        public SpawnPigeonCommand(UserStateModel userStateModel, IFactory<int> pigeonFactory)
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