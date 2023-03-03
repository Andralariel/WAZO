using DG.Tweening;
using UnityEngine;

namespace WeightSystem
{
    public class DoorOpener : Activator
    {
        [SerializeField] private float openingSpeed = 1f;
        public override void Activate()
        {
            transform.DOLocalMove(transform.localPosition + Vector3.up * transform.localScale.y, openingSpeed);
            Debug.Log("Door is Open");
        }

        public override void Deactivate()
        {
            transform.DOLocalMove(transform.localPosition - Vector3.up * transform.localScale.y, openingSpeed);
            Debug.Log("Door is Closed");
        }
    }
}
