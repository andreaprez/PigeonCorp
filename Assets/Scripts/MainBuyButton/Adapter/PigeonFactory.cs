using System.Collections.Generic;
using PigeonCorp.Hatcheries.UseCase;
using PigeonCorp.MainBuyButton.UseCase;
using PigeonCorp.MainScreen.Framework;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;
using Zenject;

namespace PigeonCorp.MainBuyButton.Adapter
{
    public class PigeonFactory : Factory.IFactory<int>
    {
        private Queue<PigeonBehaviour> _pigeonPool;
        
        private readonly PigeonTitleData _pigeonConfig;
        private readonly PigeonBehaviour _pigeonPrefab;
        private readonly Transform _pigeonContainer;
        private readonly List<Transform> _pigeonDestinations;
        private readonly UC_GetRandomBuiltHatcheryId _getRandomBuiltHatcheryIdUC;

        public PigeonFactory(
            PigeonTitleData pigeonConfig,
            UC_GetPigeonsContainer getPigeonsContainerUC,
            UC_GetPigeonDestinations getPigeonDestinationsUC,
            UC_GetRandomBuiltHatcheryId getRandomBuiltHatcheryIdUC
        )
        {
            _pigeonConfig = pigeonConfig;
            _pigeonContainer = getPigeonsContainerUC.Execute();
            _pigeonDestinations = getPigeonDestinationsUC.Execute();
            _getRandomBuiltHatcheryIdUC = getRandomBuiltHatcheryIdUC;
            
            _pigeonPrefab = ProjectContext.Instance.Container.Resolve<PigeonBehaviour>();
            
            InitializePool();
        }

        private void InitializePool()
        {
            _pigeonPool = new Queue<PigeonBehaviour>();
            
            for (int i = 0; i < _pigeonConfig.PoolSize; i++)
            {
                PigeonBehaviour pigeonInstance = Object.Instantiate(_pigeonPrefab, _pigeonContainer);
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