using PigeonCorp.Hatcheries;
using PigeonCorp.MainScreen;
using UnityEngine;

namespace PigeonCorp.Factory
{
    public class PigeonFactory : IFactory<int>
    {
        private readonly PigeonBehaviour _pigeonPrefab;
        private readonly Transform _container;
        private readonly HatcheriesModel _hatcheriesModel;

        public PigeonFactory(
            PigeonBehaviour prefab,
            Transform container,
            HatcheriesModel hatcheriesModel
        )
        {
            _pigeonPrefab = prefab;
            _container = container;
            _hatcheriesModel = hatcheriesModel;
        }
        
        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var destinationHatchery = _hatcheriesModel.GetRandomBuiltHatchery();

                PigeonBehaviour pigeon = Object.Instantiate(_pigeonPrefab, _container);
                pigeon.Initialize(destinationHatchery);
            }
        }
    }
}