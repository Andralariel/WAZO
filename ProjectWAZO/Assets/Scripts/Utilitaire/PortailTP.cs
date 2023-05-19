using System.Collections;
using UnityEngine;

namespace Utilitaire
{
    public class PortailTP : MonoBehaviour
    {
        public PortailTP associatedPortail;
        public bool canUse;

        private void Start()
        {
            associatedPortail.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canUse && other.gameObject.layer == 6 || other.gameObject.layer == 7)
            {
                canUse = false;
                StartCoroutine(Reload());
                associatedPortail.canUse = false;
                other.transform.position = associatedPortail.transform.position;
                other.attachedRigidbody.AddForce(Vector3.forward*25);
            }
        }

        IEnumerator Reload()
        {
            yield return new WaitForSeconds(0.5f);
            canUse = true;
            associatedPortail.canUse = true;
        }
    }
}
