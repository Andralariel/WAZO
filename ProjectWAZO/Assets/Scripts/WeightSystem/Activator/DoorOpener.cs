using DG.Tweening;
using UnityEngine;

namespace WeightSystem.Activator
{
    public class DoorOpener : Activator
    {
        [SerializeField] private float openingSpeed = 1f;
        public override void Activate()
        {
            transform.DOLocalMove(transform.localPosition + Vector3.up * transform.localScale.y, openingSpeed);
        }

        public override void Deactivate()
        {
            transform.DOLocalMove(transform.localPosition - Vector3.up * transform.localScale.y, openingSpeed);
        }
    }
}
