using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeightSystem.Detector;

public class BoutonBigDoor : WeightDetector
{
    [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material materialOn;
        [SerializeField] private Material materialOff;
        [SerializeField] private int triggerWeight;
        public BigDoorOpener associatedBigDoor;
        public WeightUI associatedUI;
        public bool isOn;
        protected override void LimitCheck()
        {
            if (!isOn)
            {
                associatedUI.currentWeight = LocalWeight;
                associatedUI.maxWeight = triggerWeight;
                if (LocalWeight >= triggerWeight)
                {
                    meshRenderer.material = materialOn;
                    associatedBigDoor.currentAmount += 1;
                    associatedBigDoor.UpdateText();
                    isOn = true;
                }
            
                if (LocalWeight < triggerWeight)
                {
                    meshRenderer.material = materialOff;
                }
            }
            
        }
}


