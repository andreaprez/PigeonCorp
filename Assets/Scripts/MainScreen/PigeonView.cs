using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;

namespace PigeonCorp.MainScreen
{
    public class PigeonView : MonoBehaviour
    { 
        private List<Transform> _routePoints;
        private float _recalculationTime;
        private float _movSpeed;
        private float _rotSpeed;
        private float _elapsedTime;
        private int _targetPointIndex;
        private Transform _targetPoint;
        private Vector3 _currentDirection;

        public void Initialize(PigeonTitleData config, List<Transform> routePoints)
        {
            _routePoints = routePoints;
            _recalculationTime = config.RecalculationTime;
            _movSpeed = config.MovementSpeed;
            _rotSpeed = config.RotationSpeed;

            _elapsedTime = _recalculationTime;
            _targetPointIndex = 0;
            _targetPoint = _routePoints[_targetPointIndex];
        }

        private void Update()
        {
            CheckRecalculationTime();

            CheckRoutePointReached();

            Rotate();
            Move();
        }

        private void CheckRecalculationTime()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _recalculationTime)
            {
                _elapsedTime = 0;
                _currentDirection = CalculateDirection();
            }
        }
        
        private Vector3 CalculateDirection()
        {
            var targetForward = (_targetPoint.position - transform.position).normalized;
            var noiseX = Random.Range(-0.5f, 0.5f);
            var noiseZ = Random.Range(-0.5f, 0.5f);
            var noiseVector = new Vector3(noiseX, 0f, noiseZ);

            var forward = targetForward * 10f + noiseVector;
            return forward.normalized;
        }
        
        private void CheckRoutePointReached()
        {
            var distanceToTarget = _targetPoint.position - transform.position;
            if (Vector3.SqrMagnitude(distanceToTarget) < 0.6f)
            {
                _targetPointIndex += 1;
                if (_targetPointIndex < _routePoints.Count)
                {
                    _targetPoint = _routePoints[_targetPointIndex];
                }
                else
                {
                    Hide();
                }
            }
        }

        private void Hide()
        {
            // TODO: Don't destroy, use a pool
            Destroy(gameObject);
        }
        
        private void Rotate()
        {
            var targetRotation = Quaternion.LookRotation(_currentDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotSpeed * Time.deltaTime
            );
        }
        
        private void Move()
        {
            transform.position += _movSpeed * Time.deltaTime * _currentDirection;
        }
    }
}