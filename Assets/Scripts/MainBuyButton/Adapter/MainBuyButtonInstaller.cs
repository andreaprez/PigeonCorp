using PigeonCorp.Commands;
using PigeonCorp.Factory;
using PigeonCorp.Hatcheries;
using PigeonCorp.MainScreen.UseCase;
using PigeonCorp.MainTopBar;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.ValueModifiers;
using Zenject;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonInstaller
    {
        public void Install(
            MainBuyButtonEntity entity,
            MainTopBarEntity mainTopBarEntity,
            PigeonTitleData pigeonConfig,
            ICommand<float> subtractCurrencyCommand,
            HatcheriesEntity hatcheriesEntity,
            UC_GetPigeonsContainer getPigeonsContainerUC,
            UC_GetPigeonDestinations getPigeonDestinationsUC,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC
        )
        {
            InitEntity(entity, pigeonConfig);
            
            var pigeonFactory = new PigeonFactory(
                hatcheriesEntity,
                getPigeonsContainerUC,
                getPigeonDestinationsUC
            );
            var spawnPigeonCommand = new SpawnPigeonCommand(mainTopBarEntity, pigeonFactory);

            ProjectContext.Instance.Container
                .Resolve<MainBuyButtonMediator>()
                .Initialize(
                    entity,
                    spawnPigeonCommand,
                    subtractCurrencyCommand,
                    mainTopBarEntity,
                    pigeonConfig,
                    getMainBuyButtonModifiersUC
                );
        }
        
        private static void InitEntity(MainBuyButtonEntity entity, PigeonTitleData pigeonConfig)
        {
            entity.PigeonsPerClick = 1;
            entity.PigeonCost = pigeonConfig.Cost;
        }
    }
}