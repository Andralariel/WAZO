using UnityEngine;

public class PlateformeMoulin : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      //PlayerCollision = 6 --- Spirit = 7
      if(other.gameObject.layer != 6 && other.gameObject.layer != 7) return;
      if (other.attachedRigidbody.angularDrag > 0.9f) return;
      other.transform.SetParent(transform);
   }
   private void OnTriggerExit(Collider other)
   {
      if(other.gameObject.layer != 6) return;
      other.transform.SetParent(null);
   }
}
