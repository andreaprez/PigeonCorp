using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.MainBuyButton.UseCase
{
    public class UC_GetPigeonDestinations
    {
        private readonly List<Transform> _pigeonDestinations;

        public UC_GetPigeonDestinations(List<Transform> pigeonDestinations)
        {
            _pigeonDestinations = pigeonDestinations;
        }

        public List<Transform> Execute()
        {
            return _pigeonDestinations;
        }
    }
}