using System.Collections.Generic;
using PigeonCorp.MainScreen.Framework;

namespace PigeonCorp.MainBuyButton.UseCase
{
    public class UC_GetPigeonPrefabs
    {
        private readonly List<PigeonBehaviour> _pigeonPrefabs;

        public UC_GetPigeonPrefabs(List<PigeonBehaviour> pigeonPrefabs)
        {
            _pigeonPrefabs = pigeonPrefabs;
        }

        public List<PigeonBehaviour> Execute()
        {
            return _pigeonPrefabs;
        }
    }
}