using PigeonCorp.Persistence.UserData;
using UniRx;

namespace PigeonCorp.UserState
{
    public class UserStateModel
    {
        public readonly ReactiveProperty<float> Currency;
        public readonly ReactiveProperty<int> CurrentPigeons;

        public UserStateModel(UserStateUserData userData)
        {
            Currency = new ReactiveProperty<float>(userData.Currency);
            CurrentPigeons = new ReactiveProperty<int>(userData.CurrentPigeons);
        }
    }
}