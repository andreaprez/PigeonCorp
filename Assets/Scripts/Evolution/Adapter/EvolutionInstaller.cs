using PigeonCorp.Command;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.ValueModifiers.UseCase;
using Zenject;

namespace PigeonCorp.Evolution.Adapter
{
    public class EvolutionInstaller
    {
        public void Install(
            EvolutionEntity entity,
            EvolutionUserData data,
            EvolutionTitleData config,
            ICommand resetFarmCommand,
            UC_GetEvolutionValueModifiers getEvolutionValueModifiersUC
        )
        {
            InitEntity(entity, data, config);

            ProjectContext.Instance.Container
                .Resolve<EvolutionMediator>()
                .Initialize(
                    entity,
                    config,
                    resetFarmCommand,
                    getEvolutionValueModifiersUC
            );
        }
        
        private void InitEntity(
            EvolutionEntity entity,
            EvolutionUserData data,
            EvolutionTitleData config
        )
        {
            var currentEvolutionConfig = config.EvolutionsConfiguration[data.CurrentEvolutionId];
            entity.CurrentEggId.Value = data.CurrentEvolutionId;
            entity.SelectedEggId.Value = data.CurrentEvolutionId;
            entity.CurrentPigeonIcon.Value = currentEvolutionConfig.Icon;
            entity.CurrentPigeonName.Value = currentEvolutionConfig.Name;
            entity.CurrentEggValue.Value = currentEvolutionConfig.EggValue;
            entity.CurrentFarmValue.Value = data.CurrentFarmValue;

            if (data.CurrentEvolutionId < config.EvolutionsConfiguration.Count - 1)
            {
                var nextEvolutionConfig = config.EvolutionsConfiguration[data.CurrentEvolutionId + 1];
                entity.RequiredFarmValue.Value = nextEvolutionConfig.RequiredFarmValue;
            }
            
            for (int i = 0; i < config.EvolutionsConfiguration.Count; i++)
            {
                var evolutionConfig = config.EvolutionsConfiguration[i];
                var discovered = data.CurrentEvolutionId >= i;
                
                var evolutionEggEntity = new EvolutionEggEntity();
                evolutionEggEntity.IsDiscovered.Value = discovered;
                evolutionEggEntity.PigeonIcon.Value = evolutionConfig.Icon;
                evolutionEggEntity.PigeonName.Value = evolutionConfig.Name;
                evolutionEggEntity.EggValue.Value = evolutionConfig.EggValue;
                
                entity.EvolutionEggs.Add(evolutionEggEntity);
            }
        }
    }
}