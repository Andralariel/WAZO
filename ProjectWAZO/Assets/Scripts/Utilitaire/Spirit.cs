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
        private bool _isMoving, _waitForNextStep;
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
            Debug.Log(_nextPoint);
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

            if(_waitForNextStep) return;
            if (_linkedEvent.waypoints[_nextPoint].waitBetweenStep != 0)
            {
                _waitForNextStep = true;
                if (_linkedEvent.waypoints[_nextPoint].allSpiritsWait) _linkedEvent.SpiritWait();
                else StartCoroutine(WaitBetweenStep());
            }
            else NextStep();
        }
        
        //SpiritEvent
        public void StartEvent(SpiritEvent callingEvent)
        {
            _nextPoint = 0;
            _linkedEvent = callingEvent;
            var waypointAmount = _linkedEvent.waypoints.Length;
            if(waypointAmount==0) Dispersion();
            else
            {
                _linkedEvent.ResetStopDistance(_linkedEvent.waypoints[0].spiritStopDistance);
                switch (_linkedEvent.waypoints[0].behaviour)
                {
                    case Waypoint.Behaviour.Move:
                        if(waypointAmount > 1) ChooseFirstPoint();
                        Movement();
                        break;
                    case Waypoint.Behaviour.Gather:
                        Gathering();
                        break;
                    case Waypoint.Behaviour.Disperse:
                        Dispersion();
                        break;
                    case Waypoint.Behaviour.Disappear:
                        StartCoroutine(Disappearance());
                        break;
                    default:
                        Movement();
                        throw new ArgumentOutOfRangeException();
                }
                
                _isMoving = true;
            }
        }

        private void NextStep()
        {
            _nextPoint++;
            if (_nextPoint < _linkedEvent.waypoints.Length)
            {
                spiritAgent.speed = _linkedEvent.waypoints[_nextPoint].spiritSpeed;
                spiritAgent.stoppingDistance = _linkedEvent.waypoints[_nextPoint].spiritStopDistance;
                _linkedEvent.ResetStopDistance(spiritAgent.stoppingDistance);
                switch (_linkedEvent.waypoints[_nextPoint].behaviour)
                {
                    case Waypoint.Behaviour.Move:
                        Movement();
                        break;
                    case Waypoint.Behaviour.Gather:
                        Gathering();
                        break;
                    case Waypoint.Behaviour.Disperse:
                        Dispersion();
                        break;
                    case Waypoint.Behaviour.Disappear:
                        StartCoroutine(Disappearance());
                        break;
                    default:
                        Movement();
                        throw new ArgumentOutOfRangeException();
                }

                if (_nextPoint + 1 != _linkedEvent.waypoints.Length) return;
                if (_linkedEvent.disappearDuringLastStep) StartCoroutine(Disappearance());
            }
            
            //After final step
            else
            {
                _isMoving = false;
                if(_linkedEvent.disappearOnEnd) StartCoroutine(Disappearance());
                if(_linkedEvent.waypoints[^1].behaviour == Waypoint.Behaviour.Move)Dispersion(false);
            }
        }
        
        private void ChooseFirstPoint()
        {
            var wpWpDistance = (_linkedEvent.waypoints[0].position - _linkedEvent.waypoints[1].position).magnitude;
            var spWpDistance = (transform.position - _linkedEvent.waypoints[1].position).magnitude;
            if (spWpDistance < wpWpDistance + spiritAgent.stoppingDistance) _nextPoint++;
        }

        private bool TooFar()
        {
            return spiritAgent.remainingDistance > _linkedEvent.StopDistance();
        }

        private void Movement()
        {
            spiritAgent.destination = _linkedEvent.waypoints[_nextPoint].position;
        }

        private void Gathering()
        {
            throw new NotImplementedException();
        }
        
        private void Dispersion(bool still = true)
        {
            float spreadAngle;
            Vector3 spreadDirection;
            if (still)
            {
                spreadAngle = Random.Range(0f, 1f)*2*Mathf.PI;
                spreadDirection = transform.position + new Vector3(MathF.Cos(spreadAngle), 0, Mathf.Sin(spreadAngle)).normalized*10;
            }
            else
            {
                var rangeAngle = Mathf.Deg2Rad * Vector3.SignedAngle(_linkedEvent.waypoints[^1].position-_linkedEvent.waypoints[^2].position,Vector3.forward,Vector3.up);
                Debug.Log(rangeAngle);
                spreadAngle = Random.Range(0f, 1f)*Mathf.PI + rangeAngle;
                spreadDirection = transform.position + new Vector3(MathF.Cos(spreadAngle), 0, Mathf.Sin(spreadAngle)).normalized*10;
                //Debug.Log(spreadDirection - transform.position);
            }
            spiritAgent.SetDestination(spreadDirection);
        }

        public void AllSpiritStartToWait()
        {
            StartCoroutine(WaitBetweenStep());
        }
        
        private IEnumerator WaitBetweenStep()
        {
            yield return new WaitForSeconds(_linkedEvent.waypoints[_nextPoint].waitBetweenStep);
            NextStep();
            yield return new WaitForEndOfFrame();
            _waitForNextStep = false;
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
