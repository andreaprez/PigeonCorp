using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.Hatcheries
{
    public class HatcheriesView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Text _maxCapacityText;
        [SerializeField] private Image _maxCapacityBar;
        [Space]
        [SerializeField] private List<HatcheryView> _hatcheryViews;

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
        
        public void UpdateMaxCapacityText(float capacity)
        {
            _maxCapacityText.text = capacity.ToString();
        }

        public void UpdateMaxCapacityBar(float percentage)
        {
            _maxCapacityBar.fillAmount = percentage;
        }

        public HatcheryView GetHatcheryView(int id)
        {
            return _hatcheryViews[id];
        }
        
        public void SetHatcheryBuilt(int hatcheryId, bool built)
        {
            _hatcheryViews[hatcheryId].SetBuilt(built);
        }

        public void HideHatcheryUpgradeUI(int hatcheryId)
        {
            _hatcheryViews[hatcheryId].HideUpgradeUI();
        }
        
        public void SetHatcheryName(int hatcheryId, string name)
        {
            _hatcheryViews[hatcheryId].SetName(name);
        }

        public void SetHatcheryIcon(int hatcheryId, Sprite icon)
        {
            _hatcheryViews[hatcheryId].SetIcon(icon);
        }
        
        public void SetHatcheryCost(int hatcheryId, float cost)
        {
            _hatcheryViews[hatcheryId].SetCost(cost);
        }
        
        public void UpdateHatcheryMaxCapacity(int hatcheryId, int capacity)
        {
            _hatcheryViews[hatcheryId].UpdateMaxCapacity(capacity);
        }
        
        public void UpdateHatcheryCapacityPercentage(int hatcheryId, float percentage)
        {
            _hatcheryViews[hatcheryId].UpdateCapacityPercentage(percentage);
        }

        public void SetButtonInteractable(int hatcheryId, bool interactable)
        {
            _hatcheryViews[hatcheryId].SetButtonsInteractable(interactable);
        }
    }
}