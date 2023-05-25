using UnityEngine;

namespace Interaction
{
    public class PotBehaviour : MonoBehaviour
    {
        [SerializeField] private BoxCollider col;

        private bool _potIsFull;
        
        private void Start()
        {
            if (Vector3.Dot(transform.up, Vector3.up) < 0.98)
            {
                this.enabled = false;
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_potIsFull) return;
            if (other.gameObject.layer != 14) return; //14 = object
            if (other.attachedRigidbody.isKinematic) return;
            
            other.transform.SetParent(transform);
            other.GetComponent<carotteManager>().IsPlanted(this);
            _potIsFull = true;
        }

        public void EmptyPot()
        {
            _potIsFull = false;
        }
    }
}
