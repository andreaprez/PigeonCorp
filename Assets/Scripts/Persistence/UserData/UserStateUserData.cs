using System;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class UserStateUserData
    {
        public readonly float Currency;
        public readonly int CurrentPigeons;


        public UserStateUserData(UserStateTitleData config)
        {
            Currency = config.InitialCurrency;
            CurrentPigeons = config.InitialPigeons;
        }
        
        public UserStateUserData(UserStateModel model)
        {
            Currency = model.Currency.Value;
            CurrentPigeons = model.CurrentPigeons.Value;
        }
    }
}