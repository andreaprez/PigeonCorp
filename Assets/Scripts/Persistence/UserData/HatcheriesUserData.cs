using System;
using System.Collections.Generic;
using PigeonCorp.Hatcheries;
using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Persistence.UserData
{
    [Serializable]
    public class HatcheriesUserData
    {
        public readonly List<HatcheryState> Hatcheries;

        
        public HatcheriesUserData(HatcheriesTitleData config)
        {
            Hatcheries = config.InitialHatcheries;
        }
        
        public HatcheriesUserData(HatcheriesEntity entity)
        {
            Hatcheries = new List<HatcheryState>();
            
            for (int i = 0; i < entity.Hatcheries.Count; i++)
            {
                var hatcheryModel = entity.Hatcheries[i];
                var hatcheryData = new HatcheryState
                {
                    Built = hatcheryModel.Built.Value,
                    Level = hatcheryModel.Level.Value
                };
                Hatcheries.Add(hatcheryData);
            }
        }
    }
}