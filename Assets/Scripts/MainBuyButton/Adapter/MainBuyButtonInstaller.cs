using PigeonCorp.Command;
using PigeonCorp.Hatcheries.Entity;
using PigeonCorp.MainBuyButton.Entity;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.ValueModifiers.UseCase;
using Zenject;

namespace PigeonCorp.MainBuyButton.Adapter
{
    public class MainBuyButtonInstaller
    {
        public void Install(
            MainBuyButtonEntity entity,
            PigeonTitleData pigeonConfig,
            MainTopBarEntity mainTopBarEntity,
            HatcheriesEntity hatcheriesEntity,
            ICommand<float> subtractCurrencyCommand,
            ICommand spawnPigeonCommand,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC
        )
        {
            InitEntity(entity, pigeonConfig);

            ProjectContext.Instance.Container
                .Resolve<MainBuyButtonMediator>()
                .Initialize(
                    entity,
                    pigeonConfig,
                    mainTopBarEntity,
                    hatcheriesEntity,
                    spawnPigeonCommand,
                    subtractCurrencyCommand,
                    getMainBuyButtonModifiersUC
                );
        }
        
        private void InitEntity(MainBuyButtonEntity entity, PigeonTitleData pigeonConfig)
        {
            entity.PigeonsPerClick = 1;
            entity.PigeonCost = pigeonConfig.Cost;
        }
    }
}