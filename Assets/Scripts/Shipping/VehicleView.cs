using System;
using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.Shipping
{
    public class VehicleView : MonoBehaviour
    {
        [SerializeField] private GameObject _emptyView;
        [SerializeField] private GameObject _obtainedView;
        [Space]
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _upgradeButton;
        [Space]
        [SerializeField] private Text _vehicleName;
        [SerializeField] private Image _vehicleIcon;
        [SerializeField] private Image _shippingRateBar;
        [SerializeField] private Text _shippingRateText;
        [SerializeField] private Text _purchaseCostText;
        [SerializeField] private Text _upgradeCostText;
        
        
        public IObservable<Unit> GetPurchaseButtonAsObservable()
        {
            return _purchaseButton.OnClickAsObservable();
        }
        
        public IObservable<Unit> GetUpgradeButtonAsObservable()
        {
            return _upgradeButton.OnClickAsObservable();
        }
        
        public void SetPurchased(bool purchased)
        {
            _emptyView.SetActive(!purchased);
            _obtainedView.SetActive(purchased);
        }

        public void HideUpgradeUI()
        {
            _upgradeButton.gameObject.SetActive(false);
            _purchaseCostText.transform.parent.gameObject.SetActive(false);
            _upgradeCostText.transform.parent.gameObject.SetActive(false);
        }
        
        public void SetName(string name)
        {
            _vehicleName.text = name;
        }
        
        public void SetIcon(Sprite icon)
        {
            _vehicleIcon.sprite = icon;
        }
        
        public void SetCost(float cost)
        {
            _purchaseCostText.text = cost.ToString();
            _upgradeCostText.text = cost.ToString();
        }

        public void UpdateMaxShippingRate(int rate)
        {
            _shippingRateText.text = rate.ToString();
        }
        
        public void UpdateShippingRatePercentage(float percentage)
        {
            _shippingRateBar.fillAmount = percentage;
        }
        
        public void SetButtonsInteractable(bool interactable)
        {
            _purchaseButton.interactable = interactable;
            _upgradeButton.interactable = interactable;
        }
    }
}