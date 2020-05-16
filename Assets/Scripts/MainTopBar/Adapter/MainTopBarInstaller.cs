using PigeonCorp.Persistence.UserData;
using Zenject;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarInstaller
    {
        public void Install(MainTopBarEntity entity, UserStateUserData userStateData)
        {
            InitEntity(entity, userStateData);
            
            ProjectContext.Instance.Container
                .Resolve<MainTopBarMediator>()
                .Initialize(entity);
        }

        private void InitEntity(MainTopBarEntity entity, UserStateUserData userStateData)
        {
            entity.Currency.Value = userStateData.Currency;
            entity.PigeonsCount.Value = userStateData.CurrentPigeons;
        }
    }
}