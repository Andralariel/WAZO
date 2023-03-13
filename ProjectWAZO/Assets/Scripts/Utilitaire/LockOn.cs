using Unity.VisualScripting;
using UnityEngine;

namespace Utilitaire
{
    public class LockOn : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            //PlayerCollision = 6 --- Object = 7
            if(other.gameObject.layer != 6 && other.gameObject.layer != 7) return;
            other.transform.SetParent(transform);
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.layer != 6 && other.gameObject.layer != 7) return;
            other.transform.SetParent(null);
        }
    }
}
