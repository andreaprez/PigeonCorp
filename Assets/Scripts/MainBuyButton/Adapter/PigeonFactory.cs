using System.Collections.Generic;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Hatcheries.UseCase;
using PigeonCorp.MainBuyButton.UseCase;
using PigeonCorp.MainScreen.Framework;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;

namespace PigeonCorp.MainBuyButton.Adapter
{
    public class PigeonFactory : Factory.IFactory<int>
    {
        private Queue<PigeonBehaviour> _pigeonPool;
        
        private readonly PigeonTitleData _pigeonConfig;
        private readonly List<PigeonBehaviour> _pigeonPrefabs;
        private readonly Transform _pigeonContainer;
        private readonly List<Transform> _pigeonDestinations;
        private readonly UC_GetRandomBuiltHatcheryId _getRandomBuiltHatcheryIdUC;
        private readonly EvolutionEntity _evolutionEntity;

        public PigeonFactory(
            PigeonTitleData pigeonConfig,
            UC_GetPigeonPrefabs getPigeonPrefabsUC,
            UC_GetPigeonsContainer getPigeonsContainerUC,
            UC_GetPigeonDestinations getPigeonDestinationsUC,
            UC_GetRandomBuiltHatcheryId getRandomBuiltHatcheryIdUC,
            EvolutionEntity evolutionEntity
        )
        {
            _pigeonConfig = pigeonConfig;
            _pigeonPrefabs = getPigeonPrefabsUC.Execute();
            _pigeonContainer = getPigeonsContainerUC.Execute();
            _pigeonDestinations = getPigeonDestinationsUC.Execute();
            _getRandomBuiltHatcheryIdUC = getRandomBuiltHatcheryIdUC;
            _evolutionEntity = evolutionEntity;
            
            InitializePool();
        }

        private void InitializePool()
        {
            _pigeonPool = new Queue<PigeonBehaviour>();
            var currentPigeonEvolution = _evolutionEntity.CurrentEggId.Value;
            
            for (int i = 0; i < _pigeonConfig.PoolSize; i++)
            {
                PigeonBehaviour pigeonInstance = Object.Instantiate(_pigeonPrefabs[currentPigeonEvolution], _pigeonContainer);
                pigeonInstance.gameObject.SetActive(false);
                _pigeonPool.Enqueue(pigeonInstance);
            }
        }

        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var hatcheryId = _getRandomBuiltHatcheryIdUC.Execute();
                var destinationHatchery = _pigeonDestinations[hatcheryId];

                PigeonBehaviour pigeon = _pigeonPool.Dequeue();
                pigeon.transform.position = _pigeonContainer.position;
                pigeon.transform.rotation = Quaternion.identity;
                pigeon.gameObject.SetActive(true);
                pigeon.Initialize(destinationHatchery);
                
                _pigeonPool.Enqueue(pigeon);
            }
        }
    }
}