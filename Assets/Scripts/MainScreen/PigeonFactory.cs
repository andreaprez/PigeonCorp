using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;

namespace PigeonCorp.MainScreen
{
    public class PigeonFactory
    {
        private readonly PigeonView _pigeonPrefab;
        private readonly Transform _container;
        private readonly List<Transform> _pigeonRoutePoints;
        private readonly PigeonTitleData _config;

        public PigeonFactory(
            PigeonView prefab,
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
                PigeonView pigeon = Object.Instantiate(_pigeonPrefab, _container);
                pigeon.Initialize(_config, _pigeonRoutePoints);
            }
        }
    }
}