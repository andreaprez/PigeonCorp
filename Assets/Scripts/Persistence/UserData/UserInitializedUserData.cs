using System;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class UserInitializedUserData
    {
        public bool IsInitialized;

        public UserInitializedUserData(bool initialized)
        {
            IsInitialized = initialized;
        }
    }
}