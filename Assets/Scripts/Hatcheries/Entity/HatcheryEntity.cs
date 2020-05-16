using UniRx;
using UnityEngine;

namespace PigeonCorp.Hatcheries.Entity
{
    public class HatcheryEntity
    {
        public int Id;
        
        public readonly ReactiveProperty<bool> Built;
        public readonly ReactiveProperty<int> Level;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> NextCost;
        public readonly ReactiveProperty<int> MaxCapacity;
        public readonly ReactiveProperty<int> UsedCapacity;
        public readonly ReactiveProperty<float> EggLayingRate;
        
        public HatcheryEntity()
        {
            Built = new ReactiveProperty<bool>();
            Level = new ReactiveProperty<int>();
            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            NextCost = new ReactiveProperty<float>();
            MaxCapacity = new ReactiveProperty<int>();
            UsedCapacity = new ReactiveProperty<int>();
            EggLayingRate = new ReactiveProperty<float>();
        }

        public void Build()
        {
            Built.Value = true;
            Level.Value = 1;
        }
        
        public void Upgrade()
        {
            Level.Value += 1;
        }
    }
}