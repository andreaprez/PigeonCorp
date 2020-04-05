using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.MainBuyButton
{
    public class MainBuyButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        public IObservable<Unit> GetButtonAsObservable()
        {
            return _button.OnClickAsObservable();
        }
    }
}