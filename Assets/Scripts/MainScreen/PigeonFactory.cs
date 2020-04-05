using UnityEngine;

namespace PigeonCorp.MainScreen
{
    public class PigeonFactory
    {
        private readonly PigeonView _pigeonPrefab;
        private readonly Transform _container;
        
        public PigeonFactory(PigeonView prefab, Transform container)
        {
            _pigeonPrefab = prefab;
            _container = container;
        }
        
        public void Create(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Object.Instantiate(_pigeonPrefab, _container);
            }
        }
    }
}