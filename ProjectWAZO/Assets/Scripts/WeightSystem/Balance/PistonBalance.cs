using DG.Tweening;
using UnityEngine;
using WeightSystem.Detector;

namespace WeightSystem.Balance
{
    public class PistonBalance : Balance
    {
        public PistonTrigger linkedTrigger;
        public PistonBalance opposingPiston;
        
        [SerializeField] private float halfHeight;
        [SerializeField] private float movingSpeed = 2f;
        
        private Vector3 _startPos;
        private Tweener _lastTween;
        private float _duration;

        private void Awake()
        {
            _startPos = transform.localPosition;
            if (halfHeight == 0f) halfHeight = transform.localScale.y / 2;
        }

        public override void HighState()
        {
            NewMovement();
            _duration = (transform.position-(_startPos + Vector3.up * halfHeight)).magnitude/halfHeight * movingSpeed;
            _lastTween = transform.DOLocalMove(_startPos + Vector3.up * halfHeight, _duration).OnComplete(MovementCompleted);
        }

        public override void MiddleState()
        {
            NewMovement();
            _duration = (transform.position-_startPos).magnitude/halfHeight * movingSpeed;
            _lastTween = transform.DOLocalMove(_startPos, _duration).OnComplete(MovementCompleted);
        }

        public override void LowState()
        {
            NewMovement();
            _duration = (transform.position-(_startPos - Vector3.up * halfHeight)).magnitude/halfHeight * movingSpeed;
            _lastTween = transform.DOLocalMove(_startPos - Vector3.up * halfHeight, _duration).OnComplete(MovementCompleted);
        }

        private void NewMovement()
        {
            _lastTween?.Kill();
            if(linkedTrigger.characterOnDetector) Controller.instance.onHeightChangingPlatform = true;
        }
        
        private void MovementCompleted()
        {
            _lastTween = null;
            if(linkedTrigger.characterOnDetector) Controller.instance.onHeightChangingPlatform = false;
        }
    }
}
