using System.Collections;
using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping.Framework
{
    public class VehicleBehaviour : MonoBehaviour
    {
        private float _speed;
        private float _timeToHide;
        
        public void Initialize(ShippingTitleData config)
        {
            _speed = config.VehicleSpeed;
            _timeToHide = config.TimeToHideVehicle;

            MainThreadDispatcher.StartCoroutine(HideInTime());
        }

        private IEnumerator HideInTime()
        {
            yield return new WaitForSeconds(_timeToHide);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.position += Time.deltaTime * _speed * transform.forward;
        }
    }
}