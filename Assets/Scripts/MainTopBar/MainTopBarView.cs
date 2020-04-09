using UnityEngine;
using UnityEngine.UI;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarView : MonoBehaviour
    {
        [SerializeField] private Text _currency;
        [SerializeField] private Text _pigeonsCount;

        public void UpdateCurrencyText(float currency)
        {
            _currency.text = currency.ToString();
        }
        
        public void UpdatePigeonsCountText(int count)
        {
            _pigeonsCount.text = count.ToString();
        }
    }
}