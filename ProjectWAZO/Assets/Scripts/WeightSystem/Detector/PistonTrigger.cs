using UnityEngine;
using WeightSystem.Balance;

namespace WeightSystem.Detector
{
    public class PistonTrigger : WeightDetector
    {
        [SerializeField] private PistonBalance linkedPiston;
        [SerializeField] private PistonTrigger opposingTrigger;

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            CheckState();
            opposingTrigger.CheckState();
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            CheckState();
            opposingTrigger.CheckState();
        }

        private void CheckState()
        {
            var opposingWeight = opposingTrigger.LocalWeight;
            if (LocalWeight>opposingWeight)
            {
                linkedPiston.HighState();
            }

            if (LocalWeight==opposingWeight)
            {
                linkedPiston.MiddleState();
            }

            if (LocalWeight<opposingWeight)
            {
                linkedPiston.LowState();
            }
        }
    }
}
