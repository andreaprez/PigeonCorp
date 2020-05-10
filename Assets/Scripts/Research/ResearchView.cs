using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.Research
{
    public class ResearchView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private List<BonusView> _bonusViews;
        
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
        
        public BonusView GetBonusView(int id)
        {
            return _bonusViews[id];
        }

        public void HideBonusResearchUI(int bonusId)
        {
            _bonusViews[bonusId].HideResearchUI();
        }
        
        public void SetBonusName(int bonusId, string name)
        {
            _bonusViews[bonusId].SetName(name);
        }

        public void SetBonusIcon(int bonusId, Sprite icon)
        {
            _bonusViews[bonusId].SetIcon(icon);
        }
        
        public void SetBonusCost(int bonusId, float cost)
        {
            _bonusViews[bonusId].SetCost(cost);
        }
        
        public void UpdateBonusCurrentValue(int bonusId, float value)
        {
            _bonusViews[bonusId].UpdateCurrentValue(value);
        }
        
        public void UpdateBonusNextValue(int bonusId, float value)
        {
            _bonusViews[bonusId].UpdateNextValue(value);
        }

        public void SetButtonInteractable(int bonusId, bool interactable)
        {
            _bonusViews[bonusId].SetButtonInteractable(interactable);
        }
    }
}