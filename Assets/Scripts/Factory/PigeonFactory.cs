using System.Collections.Generic;
using PigeonCorp.MainScreen;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;

namespace PigeonCorp.Factory
{
    public class PigeonFactory : IFactory<int>
    {
        private readonly PigeonBehaviour _pigeonPrefab;
        private readonly Transform _container;
        private readonly List<Transform> _pigeonRoutePoints;
        private readonly PigeonTitleData _config;

        public PigeonFactory(
            PigeonBehaviour prefab,
            Transform container,
            List<Transform> pigeonRoutePoints,
            PigeonTitleData config
        )
        {
            _pigeonPrefab = prefab;
            _container = container;
            _pigeonRoutePoints = pigeonRoutePoints;
            _config = config;
        }
        
        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                PigeonBehaviour pigeon = Object.Instantiate(_pigeonPrefab, _container);
                pigeon.Initialize(_config, _pigeonRoutePoints);
            }
        }
    }
}