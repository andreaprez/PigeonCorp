using System;
using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.Shipping
{
    public class ShippingView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Text _maxShippingRateText;
        [SerializeField] private Image _maxShippingRateBar;
        [Space]
        [SerializeField] private List<VehicleView> _vehicleViews;

        public IObservable<Unit> GetOpenButtonAsObservable()
        {
            return _openButton.OnClickAsObservable();
        }
        
        public IObservable<Unit> GetCloseButtonAsObservable()
        {
            return _closeButton.OnClickAsObservable();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateMaxShippingRateText(float rate)
        {
            _maxShippingRateText.text = rate.ToString();
        }

        public void UpdateMaxShippingRateBar(float percentage)
        {
            _maxShippingRateBar.fillAmount = percentage;
        }

        public VehicleView GetVehicleView(int id)
        {
            return _vehicleViews[id];
        }
        
        public void SetVehiclePurchased(int vehicleId, bool purchased)
        {
            _vehicleViews[vehicleId].SetPurchased(purchased);
        }

        public void SetVehiclePrefab(int hatcheryId, VehicleBehaviour prefab)
        {
            _vehicleViews[hatcheryId].SetVehiclePrefab(prefab);
        }
        
        public void SpawnVehicleInWorld(int vehicleId, ShippingTitleData config)
        {
            _vehicleViews[vehicleId].SpawnVehicleInWorld(config);
        }

        public void HideVehicleUpgradeUI(int vehicleId)
        {
            _vehicleViews[vehicleId].HideUpgradeUI();
        }
        
        public void SetVehicleName(int vehicleId, string name)
        {
            _vehicleViews[vehicleId].SetName(name);
        }

        public void SetVehicleIcon(int vehicleId, Sprite icon)
        {
            _vehicleViews[vehicleId].SetIcon(icon);
        }
        
        public void SetVehicleCost(int vehicleId, float cost)
        {
            _vehicleViews[vehicleId].SetCost(cost);
        }
        
        public void UpdateVehicleMaxShippingRate(int vehicleId, int rate)
        {
            _vehicleViews[vehicleId].UpdateMaxShippingRate(rate);
        }
        
        public void UpdateVehicleShippingRatePercentage(int vehicleId, float percentage)
        {
            _vehicleViews[vehicleId].UpdateShippingRatePercentage(percentage);
        }

        public void SetButtonInteractable(int vehicleId, bool interactable)
        {
            _vehicleViews[vehicleId].SetButtonsInteractable(interactable);
        }
    }
}