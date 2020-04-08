using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "UserStateInitialData", menuName = "PigeonCorp/UserData/UserState")]
    public class UserStateInitialData : ScriptableObject
    {
        public int CurrentPigeons;
        public int Currency;
    }
}