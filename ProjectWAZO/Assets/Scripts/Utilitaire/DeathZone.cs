using System.Collections;
using _3C;
using UnityEngine;

namespace Utilitaire
{
   public class DeathZone : MonoBehaviour
   {
      public Vector3 respawnPoint;
      public float timeToChangeRespawn;
      public float timer;

      public void Update()
      {
         if (Controller.instance.isGrounded)
         {
            timer += Time.deltaTime;
         }
         
         if (timer >= timeToChangeRespawn)
         {
            respawnPoint = Controller.instance.transform.position;
            timer = 0;
         }
      }

      private void OnTriggerEnter(Collider other)
      {
         switch (other.gameObject.layer)
         {
            //Character
            case 6:
               StartCoroutine(RespawnPlayer());
               break;
            //Spirit
            case 7:
               other.GetComponent<Spirit>()?.Respawn();
               break;
            //Object
            case 14:
               StartCoroutine(MakeObjectDisappear(other.gameObject));
               break;
         }
      }
   
   
   
      IEnumerator RespawnPlayer()
      {
         KeyUI.instance.FadeInBlackScreen(0.5f);
         CinématiqueManager.instance.isCinématique = true;
         Controller.instance.canMove = false;
         Controller.instance.canJump = false;
         Controller.instance.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
         yield return new WaitForSeconds(0.6f);
         Controller.instance.transform.position = respawnPoint + new Vector3(0, 2, 0);
         yield return new WaitForSeconds(0.6f);
         KeyUI.instance.FadeOutBlackScreen(0.5f);
         yield return new WaitForSeconds(0.5f);
         CinématiqueManager.instance.isCinématique = true;
         Controller.instance.rb.constraints = RigidbodyConstraints.FreezeRotation;
         CinématiqueManager.instance.isCinématique = false;
         Controller.instance.canMove = true;
         Controller.instance.canJump = true;
      }

      private static IEnumerator MakeObjectDisappear(GameObject other)
      {
         yield return new WaitForSeconds(1);
         other.SetActive(false);
      }
   }
}
