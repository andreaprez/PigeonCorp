using System;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "HatcheriesTitleData", menuName = "PigeonCorp/GameConfig/Hatcheries")]
    public class HatcheriesTitleData : ScriptableObject
    {
        public List<HatcheryConfig> HatcheriesConfiguration;
        public List<HatcheryState> InitialHatcheries;
    }
    
    [Serializable]
    public class HatcheryConfig
    {
        public string Name;
        public Sprite Icon;
        public float Cost;
        public int MaxCapacity;
        public int EggLayingRate;
    }

    [Serializable]
    public class HatcheryState
    {
        public bool Built;
        public int Level;
    }
}