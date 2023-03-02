using UnityEngine;
using WeightSystem;

namespace Spirits
{
    public class SpiritAltar : MonoBehaviour
    {
        private enum SpiritType
        {
            Wind,
            Earth
        }
        
        [SerializeField] private SpiritType spiritType;
        [SerializeField] private int spiritSlots;
        [SerializeField] private Activator linkedObject;
        
        
        private int _spiritAmount;
        
        public virtual void OnTriggerEnter(Collider other)
        {
            if ((int)other.attachedRigidbody.drag != (int)spiritType) return;
            _spiritAmount++;
            if(_spiritAmount==spiritSlots) linkedObject.Activate();
        }

        public virtual void OnTriggerExit(Collider other)
        {
            if ((int)other.attachedRigidbody.drag != (int)spiritType) return;
            _spiritAmount--;
            if(_spiritAmount<spiritSlots) linkedObject.Deactivate();
        }
    }
}
