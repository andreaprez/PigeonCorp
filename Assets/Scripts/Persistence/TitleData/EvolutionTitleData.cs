using System;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "EvolutionTitleData", menuName = "PigeonCorp/GameConfig/Evolution")]
    public class EvolutionTitleData : ScriptableObject
    {
        public int InitialEvolutionId;
        public List<EvolutionConfig> EvolutionsConfiguration;
    }
    
    [Serializable]
    public class EvolutionConfig
    {
        public string Name;
        public Sprite Icon;
        public float EggValue;
        public float RequiredFarmValue;
    }
}