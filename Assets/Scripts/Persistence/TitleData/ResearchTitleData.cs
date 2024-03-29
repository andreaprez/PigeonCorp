using System;
using System.Collections.Generic;
using PigeonCorp.Research.Entity;
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
        public ValueUnitType UnitType;
        public string Name;
        public List<BonusTier> Tiers;
    }
    
    [Serializable]
    public class ValueUnitType
    {
        public string Unit;
        public UnitPosition Position;
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