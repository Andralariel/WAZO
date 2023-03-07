using System.Collections.Generic;
using UnityEngine;

namespace WeightSystem.Detector
{
    public class WeightDetector : MonoBehaviour
    {
        [SerializeField] protected int LocalWeight;
        private List<Rigidbody> _rbList;

        private void Awake()
        {
            _rbList = new List<Rigidbody>();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            other.transform.SetParent(transform);
            _rbList.Add(other.attachedRigidbody);
            LocalWeight += (int)_rbList[^1].mass;
            
            //PlayerCollision
            if(other.gameObject.layer == 6) Controller.instance.SetDetector(this);
        }

        public virtual void OnTriggerExit(Collider other)
        {
            _rbList.Remove(other.attachedRigidbody);
            LocalWeight -= (int)other.attachedRigidbody.mass;
        }

        public void ResetWeight()
        {
            LocalWeight = 0;
            foreach (var rb in _rbList)
            {
                LocalWeight += (int)rb.mass;
            }
            LimitCheck();
        }

        protected virtual void LimitCheck()
        {}
    }
}
