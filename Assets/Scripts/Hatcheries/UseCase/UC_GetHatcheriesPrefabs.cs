using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Installers.Hatcheries.UseCase
{
    public class UC_GetHatcheriesPrefabs
    {
        private readonly List<GameObject> _hatcheriesPrefabs;

        public UC_GetHatcheriesPrefabs(List<GameObject> hatcheriesPrefabs)
        {
            _hatcheriesPrefabs = hatcheriesPrefabs;
        }

        public List<GameObject> Execute()
        {
            return _hatcheriesPrefabs;
        }
    }
}