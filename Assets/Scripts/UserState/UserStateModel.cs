namespace PigeonCorp.UserState
{
    public class UserStateModel
    {
        private int CurrentPigeons;
        private int Currency;

        public UserStateModel()
        {
            // TODO: Get from data / config
            CurrentPigeons = 0;
            Currency = 100;
        }
        
        public void AddPigeons(int pigeonsToAdd)
        {
            CurrentPigeons += pigeonsToAdd;
        }
    }
}