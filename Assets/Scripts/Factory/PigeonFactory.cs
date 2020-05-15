using PigeonCorp.Hatcheries;
using PigeonCorp.MainScreen;
using PigeonCorp.MainScreen.UseCase;
using UnityEngine;
using Zenject;

namespace PigeonCorp.Factory
{
    public class PigeonFactory : IFactory<int>
    {
        private readonly PigeonBehaviour _pigeonPrefab;
        private readonly Transform _pigeonContainer;
        private readonly HatcheriesModel _hatcheriesModel;

        public PigeonFactory(HatcheriesModel hatcheriesModel, UC_GetPigeonsContainer getPigeonsContainerUC)
        {
            _pigeonContainer = getPigeonsContainerUC.Execute();
            _hatcheriesModel = hatcheriesModel;
            
            _pigeonPrefab = ProjectContext.Instance.Container.Resolve<PigeonBehaviour>();
        }
        
        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var destinationHatchery = _hatcheriesModel.GetRandomBuiltHatchery();

                PigeonBehaviour pigeon = Object.Instantiate(_pigeonPrefab, _pigeonContainer);
                pigeon.Initialize(destinationHatchery);
            }
        }
    }
}