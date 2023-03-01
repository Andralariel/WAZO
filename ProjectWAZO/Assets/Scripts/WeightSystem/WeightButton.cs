using UnityEngine;

namespace WeightSystem
{
    public class WeightButton : WeightDetector
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material materialOn;
        [SerializeField] private Material materialOff;
        [SerializeField] private Activator linkedObject;
        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (LocalWeight < 5) return;
            meshRenderer.material = materialOn;
            linkedObject.Activate();
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (LocalWeight > 4) return;
            meshRenderer.material = materialOff;
            linkedObject.Deactivate();
        }
    }
}
