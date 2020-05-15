using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Installers.Hatcheries.UseCase
{
    public class UC_GetHatcheriesContainers
    {
        private readonly List<Transform> _hatcheriesContainers;

        public UC_GetHatcheriesContainers(List<Transform> hatcheriesContainers)
        {
            _hatcheriesContainers = hatcheriesContainers;
        }

        public List<Transform> Execute()
        {
            return _hatcheriesContainers;
        }
    }
}