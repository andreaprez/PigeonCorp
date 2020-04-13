using System;
using System.Collections.Generic;
using PigeonCorp.Hatchery;
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
        
        public HatcheriesUserData(HatcheriesModel model)
        {
            Hatcheries = new List<HatcheryState>();
            
            for (int i = 0; i < model.Hatcheries.Count; i++)
            {
                var hatcheryModel = model.Hatcheries[i];
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