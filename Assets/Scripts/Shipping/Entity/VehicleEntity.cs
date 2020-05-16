using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping.Entity
{
    public class VehicleEntity
    {
        public int Id;

        public readonly ReactiveProperty<bool> Purchased;
        public readonly ReactiveProperty<int> Level;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> NextCost;
        public readonly ReactiveProperty<float> MaxShippingRate;
        public readonly ReactiveProperty<float> UsedShippingRate;
        
        public VehicleEntity()
        {
            Purchased = new ReactiveProperty<bool>();
            Level = new ReactiveProperty<int>();
            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            NextCost = new ReactiveProperty<float>();
            MaxShippingRate = new ReactiveProperty<float>();
            UsedShippingRate = new ReactiveProperty<float>();
        }

        public void Purchase()
        {
            Purchased.Value = true;
            Level.Value = 1;
        }
        
        public void Upgrade()
        {
            Level.Value += 1;
        }
    }
}