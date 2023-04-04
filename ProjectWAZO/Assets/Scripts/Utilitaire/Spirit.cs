using System;
using System.Collections;
using EventSystem;
using UnityEngine;
using UnityEngine.AI;
using WeightSystem.Detector;
using Random = UnityEngine.Random;

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
            if (_nextPoint < _linkedEvent.waypoints.Length) spiritAgent.destination = _linkedEvent.waypoints[_nextPoint];
            else
            {
                _isMoving = false;
                Dispersion(false);
            }
        }
        
        //SpiritEvent
        public void SetDestination(SpiritEvent callingEvent)
        {
            _linkedEvent = callingEvent;
            var waypointAmount = _linkedEvent.waypoints.Length;
            if(waypointAmount==0) Dispersion();
            else
            {
                if(waypointAmount > 1) ChooseFirstPoint();
                spiritAgent.destination = _linkedEvent.waypoints[_nextPoint];
                _isMoving = true;
            }
        }

        private void ChooseFirstPoint()
        {
            var wpWpDistance = (_linkedEvent.waypoints[0] - _linkedEvent.waypoints[1]).magnitude;
            var spWpDistance = (transform.position - _linkedEvent.waypoints[1]).magnitude;
            if (spWpDistance < wpWpDistance + spiritAgent.stoppingDistance) _nextPoint++;
        }

        private bool TooFar()
        {
            return spiritAgent.remainingDistance > spiritAgent.stoppingDistance;
        }

        private void Dispersion(bool still = true)
        {
            float spreadAngle;
            Vector3 spreadDirection;
            if (still)
            {
                spreadAngle = Random.Range(0f, 1f)*2*Mathf.PI;
                spreadDirection = transform.position + new Vector3(MathF.Cos(spreadAngle), 0, Mathf.Sin(spreadAngle))*10;
            }
            else
            {
                var rangeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.forward, transform.forward);
                spreadAngle = Random.Range(0f, 1f)*Mathf.PI + rangeAngle;
                spreadDirection = _linkedEvent.waypoints[^1] + new Vector3(MathF.Cos(spreadAngle), 0, Mathf.Sin(spreadAngle))*10;
            }
            
            spiritAgent.destination = spreadDirection;
            StartCoroutine(Disappearance());
        }

        private IEnumerator Disappearance()
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
            gameObject.SetActive(false);
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
