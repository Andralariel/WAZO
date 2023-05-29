using _3C;
using UnityEngine;

namespace Utilitaire
{
    public class StayOnWind : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != 6) return;
            if(!Controller.instance.isPressing) return;

            Controller.instance.gravityScale = -4;
            Controller.instance.canJump = false;
            Controller.instance.isWind = true;

            if (other.attachedRigidbody.velocity.y > 0.2f) return;
            CameraController.instance.SmoothMoveFactor = 0.5f;
            CameraController.instance.canMove = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 6) return; //6 = Player
            
            CameraController.instance.canMove = true;
            CameraController.instance.filmPlayer = false;
            Controller.instance.isWind = false;
        }
    }
}
