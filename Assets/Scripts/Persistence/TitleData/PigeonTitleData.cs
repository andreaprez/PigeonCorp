using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "PigeonTitleData", menuName = "PigeonCorp/TitleData/Pigeon")]
    public class PigeonTitleData : ScriptableObject
    {
        public int MovementSpeed;
        public int RotationSpeed;
        public float RecalculationTime;
    }
}