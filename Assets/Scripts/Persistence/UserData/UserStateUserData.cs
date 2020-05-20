using System;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class UserStateUserData
    {
        public readonly float Currency;
        public readonly int CurrentPigeons;
        public readonly long LastTimeOnline;


        public UserStateUserData(UserStateTitleData config)
        {
            Currency = config.InitialCurrency;
            CurrentPigeons = config.InitialPigeons;
            LastTimeOnline = DateTime.Now.Ticks;
        }
        
        public UserStateUserData(MainTopBarEntity entity)
        {
            Currency = entity.Currency.Value;
            CurrentPigeons = entity.PigeonsCount.Value;
            LastTimeOnline = DateTime.Now.Ticks;
        }
    }
}