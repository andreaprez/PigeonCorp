using System;
using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research.Entity;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class ResearchUserData
    {
        public readonly List<BonusState> Bonuses;
        
        public ResearchUserData(ResearchTitleData config)
        {
            Bonuses = new List<BonusState>();

            foreach (var bonusType in config.BonusTypesConfiguration)
            {
                var bonusState = new BonusState()
                {
                    Type = bonusType.Type,
                    CurrentTier = 0
                };
                Bonuses.Add(bonusState);
            }
        }
        
        public ResearchUserData(ResearchEntity entity)
        {
            Bonuses = new List<BonusState>();

            foreach (var bonusType in entity.Bonuses)
            {
                var bonusData = new BonusState()
                {
                    Type = bonusType.Type,
                    CurrentTier = bonusType.Tier.Value
                };
                Bonuses.Add(bonusData);
            }
        }
    }
}