using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class VehicleModel
    {
        public readonly ReactiveProperty<bool> Purchased;
        public readonly ReactiveProperty<int> Level;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<Sprite> Icon;
        public readonly ReactiveProperty<float> NextCost;
        public readonly ReactiveProperty<float> MaxShippingRate;
        public readonly ReactiveProperty<float> UsedShippingRate;
        
        private readonly ShippingTitleData _config;

        public VehicleModel(ShippingTitleData config, VehicleState stateData)
        {
            _config = config;

            Purchased = new ReactiveProperty<bool>(stateData.Purchased);
            Level = new ReactiveProperty<int>(stateData.Level);

            Name = new ReactiveProperty<string>();
            Icon = new ReactiveProperty<Sprite>();
            NextCost = new ReactiveProperty<float>(0);
            MaxShippingRate = new ReactiveProperty<float>(0);
            UsedShippingRate = new ReactiveProperty<float>(0);
            
            if (Purchased.Value)
            {
                SetProperties();
            }
            else
            {
                NextCost.Value = config.ShippingConfiguration[Level.Value].Cost;
            }
        }

        public void Purchase()
        {
            Purchased.Value = true;
            Level.Value = 1;

            SetProperties();
        }
        
        public void Upgrade()
        {
            Level.Value += 1;
            SetProperties();
        }

        public void SetUsedShippingRate(int quantity)
        {
            UsedShippingRate.Value = quantity;
        }
        
        private void SetProperties()
        {
            Name.Value = _config.ShippingConfiguration[Level.Value - 1].Name;
            Icon.Value = _config.ShippingConfiguration[Level.Value - 1].Icon;
            MaxShippingRate.Value = _config.ShippingConfiguration[Level.Value - 1].MaxShippingRate;
            if (Level.Value < _config.ShippingConfiguration.Count)
            {
                NextCost.Value = _config.ShippingConfiguration[Level.Value].Cost;
            }
        }
    }
}