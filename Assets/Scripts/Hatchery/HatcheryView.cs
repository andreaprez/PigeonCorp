using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.Hatcheries
{
    public class HatcheryView : MonoBehaviour
    {
        [SerializeField] private GameObject _emptyView;
        [SerializeField] private GameObject _builtView;
        [Space]
        [SerializeField] private Button _buildButton;
        [SerializeField] private Button _upgradeButton;
        [Space]
        [SerializeField] private Text _hatcheryName;
        [SerializeField] private Image _hatcheryIcon;
        [SerializeField] private Image _capacityBar;
        [SerializeField] private Text _capacityText;
        [SerializeField] private Text _buildCostText;
        [SerializeField] private Text _upgradeCostText;
        
        public IObservable<Unit> GetBuildButtonAsObservable()
        {
            return _buildButton.OnClickAsObservable();
        }
        
        public IObservable<Unit> GetUpgradeButtonAsObservable()
        {
            return _upgradeButton.OnClickAsObservable();
        }
        
        public void SetBuilt(bool built)
        {
            _emptyView.SetActive(!built);
            _builtView.SetActive(built);
        }

        public void HideUpgradeUI()
        {
            _upgradeButton.gameObject.SetActive(false);
            _buildCostText.transform.parent.gameObject.SetActive(false);
            _upgradeCostText.transform.parent.gameObject.SetActive(false);
        }
        
        public void SetName(string name)
        {
            _hatcheryName.text = name;
        }
        
        public void SetIcon(Sprite icon)
        {
            _hatcheryIcon.sprite = icon;
        }
        
        public void SetCost(float cost)
        {
            _buildCostText.text = cost.ToString();
            _upgradeCostText.text = cost.ToString();
        }

        public void UpdateMaxCapacity(int capacity)
        {
            _capacityText.text = capacity.ToString();
        }
        
        public void UpdateCapacityPercentage(float percentage)
        {
            _capacityBar.fillAmount = percentage;
        }
        
        public void SetButtonsInteractable(bool interactable)
        {
            _buildButton.interactable = interactable;
            _upgradeButton.interactable = interactable;
        }
    }
}
