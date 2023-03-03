using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeightSystem
{
    public class WeightDetector : MonoBehaviour
    {
        [SerializeField] protected int LocalWeight;
        protected List<Rigidbody> RbList;

        private void Awake()
        {
            RbList = new List<Rigidbody>();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            RbList.Add(other.attachedRigidbody);
            LocalWeight += (int)RbList[^1].mass;
            
            //PlayerCollision
            if(other.gameObject.layer == 6) Controller.instance.SetDetector(this);
        }

        public virtual void OnTriggerExit(Collider other)
        {
            RbList.Remove(other.attachedRigidbody);
            LocalWeight -= (int)other.attachedRigidbody.mass;
        }

        public void ResetWeight()
        {
            LocalWeight = 0;
            foreach (var rb in RbList)
            {
                LocalWeight += (int)rb.mass;
            }
            LimitCheck();
        }
        
        public virtual void LimitCheck()
        {}
    }
}
