using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "PigeonTitleData", menuName = "PigeonCorp/TitleData/Pigeon")]
    public class PigeonTitleData : ScriptableObject
    {
        public float MovementSpeed;
        public float RotationSpeed;
        public float RecalculationTime;
        public float MovementNoise;
        public float TargetReachedOffset;

        public float Cost;
        public float EvolutionCostMultiplier;
    }
}