using UnityEngine;

namespace WeightSystem
{
    public class WeightDetector : MonoBehaviour
    {
        protected int LocalWeight;
        
        public virtual void OnTriggerEnter(Collider other)
        {
            LocalWeight += (int)other.attachedRigidbody.mass;
        }

        public virtual void OnTriggerExit(Collider other)
        {
            LocalWeight -= (int)other.attachedRigidbody.mass;
        }
    }
}
