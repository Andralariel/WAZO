using _3C;
using UnityEngine;

namespace Utilitaire
{
   public class AscendingWind : MonoBehaviour
   {
      public bool isHuge;
      public float gravityScaleToGive;
      public float AirSpeedToGive;
      public float windForce;
      public int MassFactor;
   
      private void OnTriggerStay(Collider other)
      {
         if (other.gameObject.layer == 6 && Controller.instance.isPressing)
         {
            Controller.instance.gravityScale = -4;
            Controller.instance.canJump = false;
            Controller.instance.isWind = true;
            other.attachedRigidbody.AddForce(new Vector3(0,windForce - other.attachedRigidbody.mass*MassFactor,0),ForceMode.Acceleration);
        
            if (isHuge)
            {
               CameraController.instance.isFlou = false;
               Controller.instance.isOnHugeWind = true;
               Controller.instance.hugeWindAirSpeed = AirSpeedToGive;
               Controller.instance.currentWindGravityScale = gravityScaleToGive;
            }
         }
      }
   
      private void OnTriggerExit(Collider other)
      {
         if (other.gameObject.layer != 6) return; //6 = Player
         if (!Controller.instance.isPressing) return;

         var rb = other.attachedRigidbody;
         var velocity = rb.velocity;
         rb.velocity = Vector3.Scale(velocity,new Vector3(1,0,1));
         rb.AddForce(new Vector3(0,windForce/10,0),ForceMode.VelocityChange);
         Controller.instance.isWind = false;
      }
   }
}
