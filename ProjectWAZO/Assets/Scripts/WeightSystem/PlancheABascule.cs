using System.Collections.Generic;
using _3C;
using UnityEngine;
using WeightSystem.Detector;

namespace WeightSystem
{
    public class PlancheABascule : WeightDetector
    {
        public enum HitboxType
        {
            gauche,
            droite,
            centre
        }

        public PlancheABascule master;
        public int poidGauche;
        public int poidDroite;
        public Quaternion positionNeutre;
        public Quaternion positionDroite;
        public Quaternion positionGauche;
        public float rotationSpeed;
        public HitboxType type;
        public List<Rigidbody> massListG;
        public List<Rigidbody> massListD;
        public WeightUI associatedLeftUI;
        public WeightUI associatedRightUI;

        private bool _isInPlace;
    
        void Update()
        {
            if (_isInPlace) return;
            if (type == HitboxType.centre)
            {
                if (poidDroite > poidGauche && poidDroite != poidGauche)
                {
                    transform.localRotation = Quaternion.RotateTowards(transform.localRotation,positionDroite,rotationSpeed*Time.deltaTime);
                    CheckRotation(positionDroite);
                }
                else if (poidDroite < poidGauche && poidDroite != poidGauche)
                {
                    transform.localRotation = Quaternion.RotateTowards(transform.localRotation,positionGauche,rotationSpeed*Time.deltaTime);
                    CheckRotation(positionGauche);
                }
            
                if (poidDroite == poidGauche)
                {
                    transform.localRotation = Quaternion.RotateTowards(transform.localRotation,positionNeutre,rotationSpeed*Time.deltaTime);
                    CheckRotation(positionNeutre);
                }

                associatedLeftUI.currentWeight = poidGauche;
                associatedRightUI.currentWeight = poidDroite;
          
            }
            // var transformRotation = transform.localRotation;
            // transformRotation = Quaternion.Euler(transformRotation.eulerAngles.x, 0, transformRotation.eulerAngles.z);
            // transform.localRotation = transformRotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (type == HitboxType.gauche)
            {
                if (other.gameObject.layer == 7)
                {
                    master.massListG.Add(other.attachedRigidbody);
                }
                else if(other.gameObject.layer == 6)
                {
                    master.massListG.Add(other.attachedRigidbody);
                }
            }
            else if (type == HitboxType.droite)
            {
                if (other.gameObject.layer == 7)
                {
                    master.massListD.Add(other.attachedRigidbody);
                }
                else if(other.gameObject.layer == 6)
                {
                    master.massListD.Add(other.attachedRigidbody);
                }
            }
        
            master.ResetWeight();
            Controller.instance.SetDetector(master);

            if (other.gameObject.layer == 6) master.characterOnDetector = true;
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (type == HitboxType.droite)
            {
                if (other.gameObject.layer == 7)
                {
                    master.massListD.Remove(other.attachedRigidbody);
                }
                else if(other.gameObject.layer == 6)
                {
                    master.massListD.Remove(other.attachedRigidbody);
                }
            }
            else if (type == HitboxType.gauche && other.gameObject.layer == 7 || other.gameObject.layer == 6)
            {
                if (other.gameObject.layer == 7)
                {
                    master.massListG.Remove(other.attachedRigidbody);
                }
                else if(other.gameObject.layer == 6)
                {
                    master.massListG.Remove(other.attachedRigidbody);
                }
            }
            master.ResetWeight();

            if (other.gameObject.layer == 6)
            {
                master.characterOnDetector = false;
                Controller.instance.onMovingPlank = false;
                Controller.instance.onHeightChangingPlatform = false;
            }
        }

        protected override void LimitCheck()
        {
            poidDroite = 0;
            poidGauche = 0;
            foreach (var rb in massListD)
            {
                poidDroite += (int)rb.mass;
            }
        
            foreach (var rb in massListG)
            {
                poidGauche += (int)rb.mass;
            }

            _isInPlace = false;
        }
    
        //BUG fix : ne plus bloquer le joueur dans les airs
        private void CheckRotation(Quaternion targetRotation)
        {
            var controller = Controller.instance;
            if (transform.localRotation != targetRotation)
            {
                if (!characterOnDetector) return;
                controller.onMovingPlank = true;
                controller.onHeightChangingPlatform = true;
            }
            else
            {
                _isInPlace = true;
            
                if (!characterOnDetector) return;
                controller.onMovingPlank = false;
                controller.onHeightChangingPlatform = false;
            }
        }
    }
}
