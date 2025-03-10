using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace WeightSystem.Activator
{
    public class EarthPillar : Activator
    {
        [SerializeField] private float speed;
        [SerializeField] private float waitingTime = 2;
        [SerializeField] private Vector3[] wayPoints;

        private TweenerCore<Vector3, Vector3, VectorOptions> _currentTween;
        private int _nextPoint, _numberOfPoints;
        private bool _played, _rewind, _isWaiting;

        private void Awake()
        {
            _numberOfPoints = wayPoints.Length;
        }

        public override void Activate()
        {
            if (_isWaiting) return;
            //_currentTween.Kill();
            
            _played = true;

            if (!_rewind) FindNextPoint();
        }

        
        public override void Deactivate()
        {
            _played = false;

            if (!_rewind)
            {
                _currentTween.Kill();
                StopAllCoroutines();
                _isWaiting = false;
                _rewind = true;
                FindNextPoint();
            }
        }

        private IEnumerator Movement()
        {
            _isWaiting = true;
            yield return new WaitForSeconds(waitingTime);
            _isWaiting = false;
            _currentTween = transform.DOLocalMove(wayPoints[_nextPoint], CalculateSpeed()).OnComplete(FindNextPoint);
        }

        private void FindNextPoint()
        {
            if (_rewind)
            {
                if (_nextPoint - 1 < 0)
                {
                    _rewind = false;
                    
                    if(!_played) return;
                    _nextPoint = 1;
                }
                else _nextPoint -= 1;
            }
            else
            {
                if (_nextPoint == _numberOfPoints -1)
                {
                    _rewind = true;
                    _nextPoint = _numberOfPoints-2;
                }
                else _nextPoint += 1;
            }
            
            StartCoroutine(Movement());
        }

        private float CalculateSpeed()
        {
            var distance = (wayPoints[_nextPoint] - transform.position).magnitude;
            return distance/speed;
        }
        //Add SetParent on independent script
    }
}
