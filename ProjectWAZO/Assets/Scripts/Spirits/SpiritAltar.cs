using System.Collections.Generic;
using Sound;
using UnityEngine;
using Utilitaire;
using WeightSystem;
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
        [SerializeField] private ParticleSystem vfxdrop;
        [SerializeField] private ParticleSystem vfxcomplete;
        public bool isTemple;
        public int index;
        
        private int _spiritAmount;
        private bool _activated;
        
        //Fix double activation
        [SerializeField] private List<GameObject> spiritsOnAltar;
        private static readonly int OnAltar = Animator.StringToHash("OnAltar");

        private void Start()
        {
            weightUI.SetMaxAmount(spiritSlots);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 7) return;
            if ((int)other.attachedRigidbody.drag != (int)spiritType) return;
            if (other.attachedRigidbody.angularDrag > 0.9f) return;

            spiritsOnAltar.Add(other.gameObject);
            other.gameObject.GetComponent<Spirit>().anim.SetBool(OnAltar,true);
            _spiritAmount++;
            weightUI.UpdateUI(_spiritAmount);
            vfxdrop.Play();
            
            
            if (_spiritAmount == spiritSlots)
            {
                vfxcomplete.Play();
                if (!isTemple)
                {
                    linkedObject.Activate();
                }
                _activated = true;

                if (isTemple)
                {
                    TempleManager.instance.indexCalling = index;
                    TempleManager.instance.Activate();
                    if (index == 1)
                    {
                        TempleManager.instance.Escalier1Done = true;
                    }
                    if (index == 2)
                    {
                        TempleManager.instance.Escalier2Done = true;
                    }
                }
            }

            if (_activated)  //Play sound onComplete
            {
                AudioList.Instance.PlayOneShot(AudioList.Instance.altarActive, AudioList.Instance.altarActiveVolume);
            }
            else  //Play sound spiritDropped
            {
                AudioList.Instance.PlayOneShot(AudioList.Instance.putSpiritAltar, AudioList.Instance.putSpiritAltarVolume);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 7) return;
            if ((int)other.attachedRigidbody.drag != (int)spiritType) return;

            if (!spiritsOnAltar.Contains(other.gameObject)) return;
            
            spiritsOnAltar.Remove(other.gameObject);
            other.gameObject.GetComponent<Spirit>().anim.SetBool(OnAltar,false);
            _spiritAmount--;
            weightUI.UpdateUI(_spiritAmount);
            
            if (!_activated) return;
            if (_spiritAmount < spiritSlots)
            {
                linkedObject.Deactivate();
                _activated = false;
            }
        }
    }
}
