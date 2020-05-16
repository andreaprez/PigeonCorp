using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Hatcheries.UseCase
{
    public class UC_GetHatcheryPrefabs
    {
        private readonly List<GameObject> _hatcheriesPrefabs;

        public UC_GetHatcheryPrefabs(List<GameObject> hatcheriesPrefabs)
        {
            _hatcheriesPrefabs = hatcheriesPrefabs;
        }

        public List<GameObject> Execute()
        {
            return _hatcheriesPrefabs;
        }
    }
}