using System.Collections.Generic;
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
        private readonly List<Transform> _pigeonDestinations;
        private readonly HatcheriesEntity _hatcheriesEntity;

        public PigeonFactory(
            HatcheriesEntity hatcheriesEntity,
            UC_GetPigeonsContainer getPigeonsContainerUC,
            UC_GetPigeonDestinations getPigeonDestinationsUC
        )
        {
            _pigeonContainer = getPigeonsContainerUC.Execute();
            _pigeonDestinations = getPigeonDestinationsUC.Execute();
            _hatcheriesEntity = hatcheriesEntity;
            
            _pigeonPrefab = ProjectContext.Instance.Container.Resolve<PigeonBehaviour>();
        }
        
        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var destinationHatchery = _pigeonDestinations[_hatcheriesEntity.GetRandomBuiltHatcheryId()];

                PigeonBehaviour pigeon = Object.Instantiate(_pigeonPrefab, _pigeonContainer);
                pigeon.Initialize(destinationHatchery);
            }
        }
    }
}