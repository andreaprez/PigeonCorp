using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;
using UnityEngine.AI;

namespace PigeonCorp.MainScreen.Framework
{
    public class PigeonBehaviour : MonoBehaviour
    { 
        [SerializeField] private NavMeshAgent _agent;

        public void Initialize(Transform destination)
        {
            _agent.SetDestination(destination.position);
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
            gameObject.SetActive(false);
        }
    }
}