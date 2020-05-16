using PigeonCorp.Hatcheries.Entity;

namespace PigeonCorp.Hatcheries.UseCase
{
    public class UC_GetRandomBuiltHatcheryId
    {
        private readonly HatcheriesEntity _hatcheriesEntity;

        public UC_GetRandomBuiltHatcheryId(HatcheriesEntity hatcheriesEntity)
        {
            _hatcheriesEntity = hatcheriesEntity;
        }

        public int Execute()
        {
            return _hatcheriesEntity.GetRandomBuiltHatcheryId();
        }
    }
}