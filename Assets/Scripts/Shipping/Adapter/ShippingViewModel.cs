using System.Collections.Generic;
using UniRx;

namespace PigeonCorp.Shipping.Adapter
{
    public class ShippingViewModel
    {
        public readonly ReactiveProperty<bool> IsOpen;
        public readonly ReactiveProperty<float> MaxShippingRate;
        public readonly ReactiveProperty<float> ShippingRatePercentage;
        public readonly List<VehicleViewModel> VehicleViewModels;

        public ShippingViewModel()
        {
            IsOpen = new ReactiveProperty<bool>();
            MaxShippingRate = new ReactiveProperty<float>();
            ShippingRatePercentage = new ReactiveProperty<float>();
            VehicleViewModels = new List<VehicleViewModel>();
        }
    }
}