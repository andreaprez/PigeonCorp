using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;
using UniRx;
using UnityEngine.Experimental.UIElements;

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
        
        public void AddPigeons(int pigeonsToAdd)
        {
            CurrentPigeons.Value += pigeonsToAdd;
            Gateway.Instance.UpdateUserStateData(Serialize());
        }

        public void SubtractCurrency(float price)
        {
            Currency.Value -= price;
            Gateway.Instance.UpdateUserStateData(Serialize());
        }

        private UserStateUserData Serialize()
        {
            return new UserStateUserData(this);
        }
    }
}