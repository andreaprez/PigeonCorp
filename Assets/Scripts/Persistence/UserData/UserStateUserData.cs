using System;
using PigeonCorp.UserState;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class UserStateUserData
    {
        public readonly int CurrentPigeons;
        public readonly float Currency;


        public UserStateUserData()
        {
            CurrentPigeons = 0;
            Currency = 60;
        }
        
        public UserStateUserData(UserStateModel userStateModel)
        {
            CurrentPigeons = userStateModel.CurrentPigeons.Value;
            Currency = userStateModel.Currency.Value;
        }
    }
}