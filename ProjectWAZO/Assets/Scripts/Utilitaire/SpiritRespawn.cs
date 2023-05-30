using System.Collections.Generic;
using _3C;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utilitaire
{
    public class SpiritRespawn : MonoBehaviour
    {
        //VARIABLES ************************************************************************************************************
        public List<Spirit> spiritsToRespawn;
        public float durationUntilReset = 3;
        public bool isMoulin;
        public bool isMultipleMoulin;
        public Moulin associedMoulin;
        public Moulin associedMoulin2;
        public Slider chargement;
        public CanvasGroup UI;

        [Header("DEBUG")] 
        public bool isInTrigger;
        public float holdingDuration;
    
        //FONCTIONS SYSTEMES ****************************************************************************************************
        void Start()
        {
            holdingDuration = 0f;
            chargement.maxValue = durationUntilReset;
        }

        private void Update()
        {
            chargement.value = holdingDuration;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                UI.DOFade(1, 0.5f);
                isInTrigger = true;
                PickUpObjects.instance.hasTriggerIn = true;
                PickUpObjects.instance.spiritRespawn = GetComponent<SpiritRespawn>();
            }
     
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                UI.DOFade(0, 0.5f);
                isInTrigger = false;
                holdingDuration = 0;
            }
        }
    }
}
