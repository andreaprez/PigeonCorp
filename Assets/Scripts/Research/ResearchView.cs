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
    }
}