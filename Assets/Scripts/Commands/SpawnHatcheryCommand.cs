using PigeonCorp.Factory;

namespace PigeonCorp.Commands
{
    public class SpawnHatcheryCommand : ICommand<int, int>
    {
        private readonly IFactory<int, int> _hatcheryFactory;

        public SpawnHatcheryCommand(IFactory<int, int> hatcheryFactory)
        {
            _hatcheryFactory = hatcheryFactory;
            
        }
        public void Handle(int hatcheryId, int positionId)
        {
            _hatcheryFactory.Create(hatcheryId, positionId);
        }
    }
}