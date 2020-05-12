using PigeonCorp.Factory;

namespace PigeonCorp.Commands
{
    public class SpawnVehicleCommand : ICommand<int>
    {
        private readonly IFactory<int> _vehicleFactory;

        public SpawnVehicleCommand(IFactory<int> vehicleFactory)
        {
            _vehicleFactory = vehicleFactory;
            
        }
        public void Execute(int vehicleId)
        {
            _vehicleFactory.Create(vehicleId);
        }
    }
}