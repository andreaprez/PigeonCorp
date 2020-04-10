using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Hatchery
{
    public class HatcheriesView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Text _maxCapacityText;
        [SerializeField] private Image _maxCapacityBar;
        [Space]
        [SerializeField] private List<HatcheryView> _hatcheryPanels;

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
    }
}