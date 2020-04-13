using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;
using UnityEngine.AI;

namespace PigeonCorp.MainScreen
{
    public class PigeonBehaviour : MonoBehaviour
    { 
        [SerializeField] private NavMeshAgent _agent;

        public void Initialize(Transform hatcheryEntrance)
        {
            _agent.SetDestination(hatcheryEntrance.position);
        }

        private void Update()
        {
            if (_agent.remainingDistance < 1f)
            {
                Hide();
            }
        }
        
        private void Hide()
        {
            Destroy(gameObject);
        }
    }
}