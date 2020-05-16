using UniRx;
using UnityEngine;

namespace PigeonCorp.Hatcheries.Adapter
{
    public class HatcheryViewModel
    {
        public readonly ReactiveProperty<bool> ButtonInteractable;
        public readonly ReactiveProperty<bool> UpgradeAvailable;
        public readonly ReactiveProperty<bool> Built;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> MaxCapacity;
        public readonly ReactiveProperty<float> CapacityPercentage;
        public readonly ReactiveProperty<float> Cost;

        public HatcheryViewModel()
        {
            ButtonInteractable = new ReactiveProperty<bool>();
            UpgradeAvailable = new ReactiveProperty<bool>();
            Built = new ReactiveProperty<bool>();
            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            MaxCapacity = new ReactiveProperty<float>();
            CapacityPercentage = new ReactiveProperty<float>();
            Cost = new ReactiveProperty<float>();
        }
    }
}