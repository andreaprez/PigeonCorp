using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.UserData;
using Zenject;

namespace PigeonCorp.MainTopBar.Adapter
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
            entity.LastTimeOnline = userStateData.LastTimeOnline;
            entity.Currency.Value = userStateData.Currency;
            entity.PigeonsCount.Value = userStateData.CurrentPigeons;
        }
    }
}