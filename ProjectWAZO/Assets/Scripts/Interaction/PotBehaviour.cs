using UnityEngine;

namespace Interaction
{
    public class PotBehaviour : MonoBehaviour
    {
        private void Start()
        {
            if (Vector3.Dot(transform.up, Vector3.up) < 0.98)
            {
                this.enabled = false;
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 14) return; //14 = object
            
            other.transform.SetParent(transform);
            other.GetComponent<carotteManager>().IsPlanted();
            
            //Prevent Multiple Planting
            this.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
