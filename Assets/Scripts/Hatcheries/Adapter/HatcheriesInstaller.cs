using PigeonCorp.Command;
using PigeonCorp.Hatcheries.Entity;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.ValueModifiers.UseCase;
using Zenject;

namespace PigeonCorp.Hatcheries.Adapter
{
    public class HatcheriesInstaller
    {
        public void Install(
            HatcheriesEntity entity,
            HatcheriesUserData data,
            HatcheriesTitleData config,
            MainTopBarEntity mainTopBarEntity,
            ICommand<float> subtractCurrencyCommand,
            ICommand<int, int> spawnHatcheryCommand,
            UC_GetHatcheriesValueModifiers getHatcheriesValueModifiersUC
        )
        {
            InitEntity(entity, mainTopBarEntity, data, config);

            ProjectContext.Instance.Container
                .Resolve<HatcheriesMediator>()
                .Initialize(
                    entity,
                    config,
                    mainTopBarEntity,
                    subtractCurrencyCommand,
                    spawnHatcheryCommand,
                    getHatcheriesValueModifiersUC
            );
        }
        
        private void InitEntity(
            HatcheriesEntity entity,
            MainTopBarEntity mainTopBarEntity,
            HatcheriesUserData data,
            HatcheriesTitleData config
        )
        {
            entity.MainTopBarEntity = mainTopBarEntity;
            entity.UpdateUsedCapacity();
            entity.UpdateMaxCapacity();
            entity.UpdateTotalProduction();
            
            for (int i = 0; i < data.Hatcheries.Count; i++)
            {
                var hatchery = new HatcheryEntity();
                
                hatchery.Id = i;
                hatchery.Built.Value = data.Hatcheries[i].Built;
                hatchery.Level.Value = data.Hatcheries[i].Level;
                if (hatchery.Built.Value)
                {
                    hatchery.Name.Value = config.HatcheriesConfiguration[hatchery.Level.Value - 1].Name;
                    hatchery.Icon.Value = config.HatcheriesConfiguration[hatchery.Level.Value - 1].Icon;
                    hatchery.MaxCapacity.Value = config.HatcheriesConfiguration[hatchery.Level.Value - 1].MaxCapacity;
                    hatchery.EggLayingRate.Value = config.HatcheriesConfiguration[hatchery.Level.Value - 1].EggLayingRate;
                }
                if (hatchery.Level.Value < config.HatcheriesConfiguration.Count)
                {
                    hatchery.NextCost.Value = config.HatcheriesConfiguration[hatchery.Level.Value].Cost;
                }

                entity.Hatcheries.Add(hatchery);
            }
        }
    }
}