using UnityEngine;
using WeightSystem.Balance;

namespace WeightSystem.Detector
{
    public class PistonTrigger : WeightDetector
    {
        [SerializeField] private PistonBalance linkedPiston;

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            LimitCheck();
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            LimitCheck();
        }

        protected override void LimitCheck()
        {
            CheckState();
            linkedPiston.opposingPiston.linkedTrigger.CheckState();
        }
        
        private void CheckState()
        {
            var opposingWeight = linkedPiston.opposingPiston.linkedTrigger.LocalWeight;
            if (LocalWeight<opposingWeight)
            {
                linkedPiston.HighState();
            }

            if (LocalWeight==opposingWeight)
            {
                linkedPiston.MiddleState();
            }

            if (LocalWeight>opposingWeight)
            {
                linkedPiston.LowState();
            }
        }
    }
}
