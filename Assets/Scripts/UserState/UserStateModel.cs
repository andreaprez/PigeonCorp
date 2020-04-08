using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.UserState
{
    public class UserStateModel
    {
        public int CurrentPigeons { get; private set; }
        public int Currency { get; private set; }

        public UserStateModel(UserStateUserData userData)
        {
            CurrentPigeons = userData.CurrentPigeons;
            Currency = userData.Currency;
        }
        
        public void AddPigeons(int pigeonsToAdd)
        {
            CurrentPigeons += pigeonsToAdd;
            Gateway.Instance.UpdateUserStateData(Serialize());
        }

        private UserStateUserData Serialize()
        {
            return new UserStateUserData(this);
        }
    }
}