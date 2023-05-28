using DG.Tweening;
using UnityEngine;

namespace Interaction
{
    public class carotteManager : MonoBehaviour
    {
        public Vector3 plantedRotation;
        public Vector3 plantedPosOffset;
        public Rigidbody rb;
        public BoxCollider carotteColider;
        
        private PotBehaviour _myPot;
    
        private void Start()
        { 
            carotteColider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public void IsTaken()
        {
            if (_myPot != null)
            {
                _myPot.EmptyPot();
                _myPot = null;
            }

            carotteColider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public void IsLeft()
        {
            carotteColider.enabled = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        public void IsPlanted(PotBehaviour pot)
        {
            var tran = transform;
            tran.DOLocalRotate(plantedRotation, 0.25f);
            tran.DOLocalMove(plantedPosOffset, 0.25f);
            carotteColider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
            _myPot = pot;
        }
    }
}
