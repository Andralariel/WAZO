using DG.Tweening;
using UnityEngine;

namespace WeightSystem.Activator
{
    public class DoorOpener : Activator
    {
        [SerializeField] private float openingSpeed = 1f;

        private Vector3 _startPos;

        private void Awake()
        {
            _startPos = transform.localPosition;
        }

        public override void Activate()
        {
            transform.DOLocalMove(_startPos - Vector3.up * transform.localScale.y, openingSpeed);
        }

        public override void Deactivate()
        {
            transform.DOLocalMove(_startPos, openingSpeed);
        }
    }
}
