using UnityEngine;
using WeightSystem.Balance;

namespace WeightSystem.Detector
{
    public class PistonTrigger : WeightDetector
    {
        [SerializeField] private PistonBalance linkedPiston;
        public WeightUI associatedUI;
        
        protected override void LimitCheck()
        {
            CheckState();
            linkedPiston.opposingPiston.linkedTrigger.CheckState();
        }
        
        private void CheckState()
        {
            associatedUI.UpdateUI(LocalWeight);
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
