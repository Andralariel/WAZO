using System;
using UnityEngine;
using Activator = WeightSystem.Activator.Activator;

namespace Spirits
{
    public class SpiritAltar : MonoBehaviour
    {
        private enum SpiritType
        {
            Wind,
            Earth,
            Cosmos
        }
        
        [SerializeField] private SpiritType spiritType;
        [SerializeField] private int spiritSlots;
        [SerializeField] private Activator linkedObject;
        public WeightUI weightUI;
        
        
        private int _spiritAmount;
        private bool _activated;

        private void Update()
        {
            weightUI.currentWeight = _spiritAmount;
            weightUI.maxWeight = spiritSlots;
        }

        public void OnTriggerEnter(Collider other)
        {
            
            if ((int)other.attachedRigidbody.drag != (int)spiritType) return;
            _spiritAmount++;

            if (_activated) return;
            if (_spiritAmount == spiritSlots)
            {
                linkedObject.Activate();
                _activated = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if ((int)other.attachedRigidbody.drag != (int)spiritType) return;
            _spiritAmount--;
            
            if (!_activated) return;
            if (_spiritAmount < spiritSlots)
            {
                linkedObject.Deactivate();
                _activated = false;
            }
        }
    }
}
