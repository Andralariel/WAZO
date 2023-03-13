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

        private void Awake()
        {
            _startPos = transform.localPosition;
            if (halfHeight == 0f) halfHeight = transform.localScale.y / 2;
        }

        public override void HighState()
        {
            transform.DOLocalMove(_startPos + Vector3.up * halfHeight, movingSpeed);
        }

        public override void MiddleState()
        {
            transform.DOLocalMove(_startPos, movingSpeed);
        }

        public override void LowState()
        {
            transform.DOLocalMove(_startPos - Vector3.up * halfHeight, movingSpeed);
        }
    }
}
