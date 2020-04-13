using System;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonCorp.Persistence.TitleData
{
    [CreateAssetMenu(fileName = "ShippingTitleData", menuName = "PigeonCorp/GameConfig/Shipping")]
    public class ShippingTitleData : ScriptableObject
    {
        public List<VehicleConfig> ShippingConfiguration;
        public List<VehicleState> InitialVehicles;
        public float TimeToSpawnVehicleInSeconds;
        public float VehicleSpeed;
        public float TimeToDestroyVehicle;
    }

    [Serializable]
    public class VehicleConfig
    {
        public string Name;
        public Sprite Icon;
        public float Cost;
        public int MaxShippingRate;
    }

    [Serializable]
    public class VehicleState
    {
        public bool Purchased;
        public int Level;
    }
}