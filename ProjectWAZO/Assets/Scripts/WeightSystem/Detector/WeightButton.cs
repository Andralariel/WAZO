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
        
        protected override void LimitCheck()
        {
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
