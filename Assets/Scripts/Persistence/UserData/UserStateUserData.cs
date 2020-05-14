using System;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.TitleData;

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
        
        public UserStateUserData(MainTopBarEntity model)
        {
            Currency = model.Currency.Value;
            CurrentPigeons = model.PigeonsCount.Value;
        }
    }
}