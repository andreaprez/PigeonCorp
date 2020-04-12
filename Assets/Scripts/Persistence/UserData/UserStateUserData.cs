using System;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class UserStateUserData
    {
        public readonly int CurrentPigeons;
        public readonly float Currency;


        public UserStateUserData(UserStateTitleData config)
        {
            CurrentPigeons = config.InitialPigeons;
            Currency = config.InitialCurrency;
        }
        
        public UserStateUserData(UserStateModel model)
        {
            CurrentPigeons = model.CurrentPigeons.Value;
            Currency = model.Currency.Value;
        }
    }
}