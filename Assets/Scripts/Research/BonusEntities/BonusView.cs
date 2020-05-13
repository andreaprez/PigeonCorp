using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.Research
{
    public class BonusView : MonoBehaviour
    {
        [SerializeField] private Button _researchButton;
        [Space]
        [SerializeField] private Text _bonusName;
        [SerializeField] private Image _bonusIcon;
        [SerializeField] private Text _researchCostText;
        [SerializeField] private Text _currentValueText;
        [SerializeField] private Text _nextValueText;
        [SerializeField] private Image _nextValueArrow;
        
        public IObservable<Unit> GetResearchButtonAsObservable()
        {
            return _researchButton.OnClickAsObservable();
        }
        
        public void HideResearchUI()
        {
            _researchButton.gameObject.SetActive(false);
            _researchCostText.transform.parent.gameObject.SetActive(false);
            _nextValueText.gameObject.SetActive(false);
            _nextValueArrow.gameObject.SetActive(false);
        }
        
        public void SetName(string name)
        {
            _bonusName.text = name;
        }
        
        public void SetIcon(Sprite icon)
        {
            _bonusIcon.sprite = icon;
        }
        
        public void SetCost(float cost)
        {
            _researchCostText.text = cost.ToString();
        }
        
        public void UpdateCurrentValue(string value)
        {
            _currentValueText.text = value;
        }
        
        public void UpdateNextValue(string value)
        {
            _nextValueText.text = value;
        }

        public void SetButtonInteractable(bool interactable)
        {
            _researchButton.interactable = interactable;
        }
    }
}