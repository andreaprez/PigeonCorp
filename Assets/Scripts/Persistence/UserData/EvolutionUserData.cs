using System;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class EvolutionUserData
    {
        public readonly int CurrentEvolutionId;
        public readonly float CurrentFarmValue;

        
        public EvolutionUserData(EvolutionTitleData config)
        {
            CurrentEvolutionId = config.InitialEvolutionId;
            CurrentFarmValue = 0f;
        }
        
        public EvolutionUserData(EvolutionEntity entity)
        {
            CurrentEvolutionId = entity.CurrentEggId.Value;
            CurrentFarmValue = entity.CurrentFarmValue.Value;
        }
    }
}