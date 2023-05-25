using _3C;
using DG.Tweening;
using UnityEngine;

namespace Utilitaire
{
    public class echelleData : MonoBehaviour
    {
        public enum Orientation
        {
            nord,
            sud,
            est,
            ouest
        }
        
        [SerializeField] public Orientation orientation;
        public float echelleSpeed = 7;
        private void Update()
        {
            if (Controller.instance.isEchelle && PickUpObjects.instance.currentEchelle == gameObject)
            {
                Vector3 lookAtVector = new Vector3(transform.position.x, Controller.instance.transform.position.y,
                    transform.position.z);
                Controller.instance.transform.DOLookAt(lookAtVector, 0.5f);
            
                switch (orientation)
                {
                    
                    case Orientation.nord:
                        Controller.instance.rb.velocity = transform.up * (Controller.instance.moveInput.z * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime)));
                        break;
                    case Orientation.sud:
                        Controller.instance.rb.velocity = transform.up * (-Controller.instance.moveInput.z * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime)));
                        break;
                    case Orientation.est:
                        Controller.instance.rb.velocity = transform.up * (Controller.instance.moveInput.x * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime)));
                        break;
                    case Orientation.ouest:
                        Controller.instance.rb.velocity = transform.up * (-Controller.instance.moveInput.x * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime)));
                        break;
                }
            }
        }
    }
}
