using PigeonCorp.Command;
using PigeonCorp.Factory;
using PigeonCorp.MainTopBar.Entity;

namespace PigeonCorp.MainBuyButton.UseCase
{
    public class SpawnPigeonCommand : ICommand
    {
        private readonly MainTopBarEntity _mainTopBarEntity;
        private readonly IFactory<int> _pigeonFactory;

        public SpawnPigeonCommand(MainTopBarEntity mainTopBarEntity, IFactory<int> pigeonFactory)
        {
            _mainTopBarEntity = mainTopBarEntity;
            _pigeonFactory = pigeonFactory;
        }
        
        public void Execute()
        {
            _mainTopBarEntity.AddPigeons(1);
            _pigeonFactory.Create(1);
        }
    }
}