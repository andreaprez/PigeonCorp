using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "UserStateTitleData", menuName = "PigeonCorp/GameConfig/UserState")]
    public class UserStateTitleData : ScriptableObject
    {
        public int InitialPigeons;
        public float InitialCurrency;
    }
}