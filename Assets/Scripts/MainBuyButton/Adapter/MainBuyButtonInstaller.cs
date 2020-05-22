using PigeonCorp.Command;
using PigeonCorp.MainBuyButton.Entity;
using PigeonCorp.MainBuyButton.UseCase;
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
            MainTopBarEntity mainTopBarEntity,
            PigeonTitleData pigeonConfig,
            ICommand<float> subtractCurrencyCommand,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC,
            PigeonCorp.Factory.IFactory<int> pigeonFactory
        )
        {
            InitEntity(entity, pigeonConfig);
            
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
        
        private void InitEntity(MainBuyButtonEntity entity, PigeonTitleData pigeonConfig)
        {
            entity.PigeonsPerClick = 1;
            entity.PigeonCost = pigeonConfig.Cost;
        }
    }
}