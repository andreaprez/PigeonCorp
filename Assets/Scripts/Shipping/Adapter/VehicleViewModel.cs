using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class VehicleViewModel
    {
        public readonly ReactiveProperty<bool> ButtonInteractable;
        public readonly ReactiveProperty<bool> UpgradeAvailable;
        public readonly ReactiveProperty<bool> Purchased;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> MaxShippingRate;
        public readonly ReactiveProperty<float> ShippingRatePercentage;
        public readonly ReactiveProperty<float> Cost;

        public VehicleViewModel()
        {
            ButtonInteractable = new ReactiveProperty<bool>();
            UpgradeAvailable = new ReactiveProperty<bool>();
            Purchased = new ReactiveProperty<bool>();
            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            MaxShippingRate = new ReactiveProperty<float>();
            ShippingRatePercentage = new ReactiveProperty<float>();
            Cost = new ReactiveProperty<float>();
        }
    }
}