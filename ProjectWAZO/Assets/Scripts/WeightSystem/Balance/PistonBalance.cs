using System;
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
            gameObject.layer = 0;
            transform.DOLocalMove(_startPos + Vector3.up * halfHeight, movingSpeed).OnComplete((() => gameObject.layer = 3));
        }

        public override void MiddleState()
        {
            gameObject.layer = 0;
            transform.DOLocalMove(_startPos + Vector3.up * halfHeight, movingSpeed).OnComplete((() => gameObject.layer = 3));
        }

        public override void LowState()
        {
            gameObject.layer = 0;
            transform.DOLocalMove(_startPos - Vector3.up * halfHeight, movingSpeed).OnComplete((() => gameObject.layer = 3));
        }
    }
}
