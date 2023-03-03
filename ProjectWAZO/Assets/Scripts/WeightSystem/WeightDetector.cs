using System.Collections.Generic;
using UnityEngine;

namespace WeightSystem
{
    public class WeightDetector : MonoBehaviour
    {
        protected int LocalWeight;
        private List<Rigidbody> _rbList;

        private void Awake()
        {
            _rbList = new List<Rigidbody>();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
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
