using System.Collections;
using _3C;
using Sound;
using UnityEngine;

namespace Utilitaire
{
   public class DeathZone : MonoBehaviour
   {
      private void OnTriggerEnter(Collider other)
      {
         Debug.Log("DeathZone");
         switch (other.gameObject.layer)
         {
            //Character
            case 6:
               StartCoroutine(RespawnPlayer());
               break;
            //Spirit
            case 7:
               var instance = PickUpObjects.instance;
               if (other.gameObject == instance.pickedObject)
               {
                  if(PickUpObjects.instance.isThingTaken) other.GetComponent<Spirit>()?.Respawn();
               }
               else other.GetComponent<Spirit>()?.Respawn();
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
         AudioList.Instance.PlayOneShot(AudioList.Instance.deathScream, AudioList.Instance.deathScreamVolume);
         Debug.Log("Death Scream");
         CinématiqueManager.instance.isCinématique = true;
         var instance = Controller.instance;
         instance.canMove = false;
         instance.canJump = false;
         instance.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
         yield return new WaitForSeconds(0.6f);
         instance.transform.position = instance.respawnPoint + new Vector3(0, 2, 0);
         yield return new WaitForSeconds(0.6f);
         KeyUI.instance.FadeOutBlackScreen(0.5f);
         yield return new WaitForSeconds(0.5f);
         CinématiqueManager.instance.isCinématique = true;
         instance.rb.constraints = RigidbodyConstraints.FreezeRotation;
         CinématiqueManager.instance.isCinématique = false;
         instance.canMove = true;
         instance.canJump = true;
      }

      private static IEnumerator MakeObjectDisappear(GameObject other)
      {
         yield return new WaitForSeconds(1);
         other.SetActive(false);
      }
   }
}
