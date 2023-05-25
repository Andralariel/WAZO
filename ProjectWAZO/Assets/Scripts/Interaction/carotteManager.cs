using DG.Tweening;
using UnityEngine;

namespace Interaction
{
    public class carotteManager : MonoBehaviour
    {
        public Rigidbody rb;
        public BoxCollider carotteColider;
    
        private void Start()
        { 
            carotteColider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public void IsTaken()
        {
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

        public void IsPlanted()
        {
            var tran = transform;
            tran.DOLocalRotate(Vector3.zero, 0.25f);
            tran.DOLocalMove(Vector3.zero, 0.25f);
            carotteColider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
            
            //Prevent Multiple Planting
            this.enabled = false;
        }
    }
}
