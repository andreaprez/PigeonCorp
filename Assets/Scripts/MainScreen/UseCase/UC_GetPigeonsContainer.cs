using UnityEngine;

namespace PigeonCorp.MainScreen.UseCase
{
    public class UC_GetPigeonsContainer
    {
        private readonly Transform _pigeonsContainer;

        public UC_GetPigeonsContainer(Transform pigeonsContainer)
        {
            _pigeonsContainer = pigeonsContainer;
        }

        public Transform Execute()
        {
            return _pigeonsContainer;
        }
    }
}