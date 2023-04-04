using EventSystem;
using UnityEngine;
using UnityEngine.AI;
using WeightSystem.Detector;

namespace Utilitaire
{
    public class Spirit : MonoBehaviour
    {
        public bool isTaken;
        public bool isClosest;
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent spiritAgent;
        
        private SpiritEvent _linkedEvent;
        private bool _isMoving;
        private int _nextPoint;
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 10)
            {
                isClosest = false;
            }    
        }

        private void Update()
        {
            if (isTaken)
            {
                rb.isKinematic = true;
                // meshRenderer.material = unselectedMaterial;
            }
            else
            {
                rb.isKinematic = false;
            }

            if (!_isMoving) return;
            if (TooFar()) return;
            
            _nextPoint++;
            if (_nextPoint + 1 > _linkedEvent.waypoints.Length) _isMoving = false;
            else spiritAgent.destination = _linkedEvent.waypoints[_nextPoint];
        }
        
        //SpiritEvent
        public void SetDestination(SpiritEvent callingEvent)
        {
            _linkedEvent = callingEvent;
            spiritAgent.destination = _linkedEvent.waypoints[_nextPoint];
            _isMoving = true;
        }

        private bool TooFar()
        {
            return spiritAgent.remainingDistance > spiritAgent.stoppingDistance;
        }
    
        //WeightSystem
        private WeightDetector _currentDetector;

        public void SetDetector(WeightDetector detector)
        {
            _currentDetector = detector;
        }

        public WeightDetector ResetWeightOnDetector()
        {
            if (_currentDetector == default) return default;
            _currentDetector.ResetWeight();
            return _currentDetector;
        }
    }
}
