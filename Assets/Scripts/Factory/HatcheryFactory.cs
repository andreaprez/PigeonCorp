using System.Collections.Generic;
using PigeonCorp.Installers.Hatcheries.UseCase;
using UnityEngine;

namespace PigeonCorp.Factory
{
    public class HatcheryFactory : IFactory<int, int>
    {
        private readonly List<GameObject> _hatcheryPrefabs;
        private readonly List<Transform> _hatcheryContainers;

        public HatcheryFactory(
            UC_GetHatcheryPrefabs getHatcheryPrefabsUC,
            UC_GetHatcheriesContainers getHatcheriesContainersUC
        )
        {
            _hatcheryPrefabs = getHatcheryPrefabsUC.Execute();
            _hatcheryContainers = getHatcheriesContainersUC.Execute();
        }
        
        public void Create(int prefabId, int positionId)
        {
            if (_hatcheryContainers[positionId].childCount > 0)
            {
                Object.Destroy(_hatcheryContainers[positionId].GetChild(0).gameObject);
            }
            
            var prefab = _hatcheryPrefabs[prefabId];
            var position = _hatcheryContainers[positionId];
            Object.Instantiate(prefab, position);
        }
    }
}