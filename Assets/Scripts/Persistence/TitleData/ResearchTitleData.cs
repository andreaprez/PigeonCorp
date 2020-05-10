using System;
using System.Collections.Generic;
using PigeonCorp.Research;
using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "ResearchTitleData", menuName = "PigeonCorp/GameConfig/Research")]
    public class ResearchTitleData : ScriptableObject
    {
        public List<BonusConfig> BonusTypesConfiguration;
    }

    [Serializable]
    public class BonusConfig
    {
        public BonusType Type;
        public string Name;
        public Sprite Icon;
        public List<BonusTier> Tiers;
    }
    
    [Serializable]
    public class BonusTier
    {
        public float Value;
        public float Cost;
    }

    [Serializable]
    public class BonusState
    {
        public BonusType Type;
        public int CurrentTier;
    }
}