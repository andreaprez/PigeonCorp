using PigeonCorp.Command;
using PigeonCorp.Factory;

namespace PigeonCorp.Hatcheries.UseCase
{
    public class SpawnHatcheryCommand : ICommand<int, int>
    {
        private readonly IFactory<int, int> _hatcheryFactory;

        public SpawnHatcheryCommand(IFactory<int, int> hatcheryFactory)
        {
            _hatcheryFactory = hatcheryFactory;
            
        }
        public void Execute(int hatcheryId, int positionId)
        {
            _hatcheryFactory.Create(hatcheryId, positionId);
        }
    }
}