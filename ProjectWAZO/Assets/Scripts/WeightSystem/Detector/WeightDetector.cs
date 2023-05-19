using System.Collections.Generic;
using _3C;
using UnityEngine;
using Utilitaire;

namespace WeightSystem.Detector
{
    public class WeightDetector : MonoBehaviour
    {
        protected int LocalWeight;
        public bool characterOnDetector;
        private List<Rigidbody> _rbList;

        private void Awake()
        {
            _rbList = new List<Rigidbody>();
        }

        public void OnTriggerEnter(Collider other)
        {
            _rbList.Add(other.attachedRigidbody);
            LocalWeight += (int)_rbList[^1].mass;
            
            //PlayerCollision = 6 --- Object = 7
            if (other.gameObject.layer == 6)
            {
                Controller.instance.SetDetector(this);
                characterOnDetector = true;
            }
            if(other.gameObject.layer == 7) other.GetComponent<Spirit>().SetDetector(this);
            
            LimitCheck();
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 6) characterOnDetector = false;
            
            Controller.instance.onHeightChangingPlatform = false;
            _rbList.Remove(other.attachedRigidbody);
            LocalWeight -= (int)other.attachedRigidbody.mass;
            
            LimitCheck();
        }

        //Fonction call quand le joueur lache un objet de son bec
        public void ResetWeight()
        {
            LocalWeight = 0;
            foreach (var rb in _rbList)
            {
                LocalWeight += (int)rb.mass;
            }
            LimitCheck();
        }

        //Fonction vérifiant l'état de l'objet et l'actualisant si besoin
        //A modifier dans tous les enfants !!
        protected virtual void LimitCheck()
        {}
    }
}
