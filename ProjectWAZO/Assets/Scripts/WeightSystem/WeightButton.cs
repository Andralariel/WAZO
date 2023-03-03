using UnityEngine;

namespace WeightSystem
{
    public class WeightButton : WeightDetector
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material materialOn;
        [SerializeField] private Material materialOff;
        [SerializeField] private Activator linkedObject;
        [SerializeField] private int triggerWeight;
        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (LocalWeight < triggerWeight) return;
            meshRenderer.material = materialOn;
            linkedObject.Activate();
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (LocalWeight >= triggerWeight) return;
            meshRenderer.material = materialOff;
            linkedObject.Deactivate();
        }

        public override void LimitCheck()
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
