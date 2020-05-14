using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarInstaller
    {
        public void Install(MainTopBarEntity entity, UserStateUserData userStateData)
        {
            InitEntity(entity, userStateData);
            new MainTopBarMediator(entity).Initialize();
        }

        private static void InitEntity(MainTopBarEntity entity, UserStateUserData userStateData)
        {
            entity.Currency.Value = userStateData.Currency;
            entity.PigeonsCount.Value = userStateData.CurrentPigeons;
        }
    }
}