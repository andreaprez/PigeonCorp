using System.Collections.Generic;
using PigeonCorp.Factory;
using PigeonCorp.Hatcheries.UseCase;
using UnityEngine;

namespace PigeonCorp.Hatcheries.Adapter
{
    public class HatcheryFactory : IFactory<int, int>
    {
        private List<List<GameObject>> _hatcheryInstances;

        private readonly List<GameObject> _hatcheryPrefabs;
        private readonly List<Transform> _hatcheryContainers;

        public HatcheryFactory(
            UC_GetHatcheryPrefabs getHatcheryPrefabsUC,
            UC_GetHatcheriesContainers getHatcheriesContainersUC
        )
        {
            _hatcheryPrefabs = getHatcheryPrefabsUC.Execute();
            _hatcheryContainers = getHatcheriesContainersUC.Execute();
            
            InitializePrefabInstances();
        }
        
        private void InitializePrefabInstances()
        {
            _hatcheryInstances = new List<List<GameObject>>();

            for (int positionId = 0; positionId < _hatcheryContainers.Count; positionId++)
            {
                _hatcheryInstances.Add(new List<GameObject>());
                for (int prefabId = 0; prefabId < _hatcheryPrefabs.Count; prefabId++)
                {
                    GameObject hatchery = Object.Instantiate(_hatcheryPrefabs[prefabId], _hatcheryContainers[positionId]);
                    hatchery.gameObject.SetActive(false);
                    _hatcheryInstances[positionId].Add(hatchery);
                }
            }
        }
        
        public void Create(int prefabId, int positionId)
        {
            foreach (var hatcheryPrefab in _hatcheryInstances[positionId])
            {
                if (hatcheryPrefab.activeInHierarchy)
                {
                    hatcheryPrefab.SetActive(false);
                }
            }

            GameObject hatchery = _hatcheryInstances[positionId][prefabId];
            hatchery.transform.position = _hatcheryContainers[positionId].position;
            hatchery.transform.rotation = _hatcheryContainers[positionId].rotation;
            hatchery.gameObject.SetActive(true);
        }
    }
}