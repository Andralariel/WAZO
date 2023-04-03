using UnityEngine;

namespace WeightSystem.Detector
{
    public class WeightButton : WeightDetector
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material materialOn;
        [SerializeField] private Material materialOff;
        [SerializeField] private Activator.Activator linkedObject;
        [SerializeField] private int triggerWeight;
        public WeightUI associatedUI;
        protected override void LimitCheck()
        {
            associatedUI.currentWeight = LocalWeight;
            associatedUI.maxWeight = triggerWeight;
            if (LocalWeight >= triggerWeight)
            {
                meshRenderer.material = materialOn;
                linkedObject.Activate();
            }
            
            if (LocalWeight < triggerWeight)
            {
                meshRenderer.material = materialOff;
                linkedObject.Deactivate();
            }
        }
    }
}
