using System.Collections;
using PigeonCorp.Persistence.TitleData;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Shipping
{
    public class VehicleBehaviour : MonoBehaviour
    {
        private float _speed;
        private float _timeToDestroy;
        
        public void Initialize(ShippingTitleData config)
        {
            _speed = config.VehicleSpeed;
            _timeToDestroy = config.TimeToDestroyVehicle;

            MainThreadDispatcher.StartCoroutine(DestroyInTime());
        }

        private IEnumerator DestroyInTime()
        {
            yield return new WaitForSeconds(_timeToDestroy);
            Destroy(gameObject);
        }

        private void Update()
        {
            transform.position += Time.deltaTime * _speed * transform.forward;
        }
    }
}