using System.Collections.Generic;
using PigeonCorp.Hatcheries.UseCase;
using PigeonCorp.MainBuyButton.UseCase;
using PigeonCorp.MainScreen.Framework;
using UnityEngine;
using Zenject;

namespace PigeonCorp.MainBuyButton.Adapter
{
    public class PigeonFactory : Factory.IFactory<int>
    {
        private readonly PigeonBehaviour _pigeonPrefab;
        private readonly Transform _pigeonContainer;
        private readonly List<Transform> _pigeonDestinations;
        private readonly UC_GetRandomBuiltHatcheryId _getRandomBuiltHatcheryIdUC;


        public PigeonFactory(
            UC_GetPigeonsContainer getPigeonsContainerUC,
            UC_GetPigeonDestinations getPigeonDestinationsUC,
            UC_GetRandomBuiltHatcheryId getRandomBuiltHatcheryIdUC
        )
        {
            _pigeonContainer = getPigeonsContainerUC.Execute();
            _pigeonDestinations = getPigeonDestinationsUC.Execute();
            _getRandomBuiltHatcheryIdUC = getRandomBuiltHatcheryIdUC;
            
            _pigeonPrefab = ProjectContext.Instance.Container.Resolve<PigeonBehaviour>();
        }
        
        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var hatcheryId = _getRandomBuiltHatcheryIdUC.Execute();
                var destinationHatchery = _pigeonDestinations[hatcheryId];

                PigeonBehaviour pigeon = Object.Instantiate(_pigeonPrefab, _pigeonContainer);
                pigeon.Initialize(destinationHatchery);
            }
        }
    }
}