using System.Collections;
using _3C;
using UnityEngine;

namespace Utilitaire
{
    public class InnerDeathZone : MonoBehaviour
    {
        private bool _characterIsInside;
      
      private void OnTriggerEnter(Collider other)
      {
         if (other.gameObject.layer != 6) return;
         
         _characterIsInside = true;
         StartCoroutine(CharacterBuffer());
      }

      private void OnTriggerExit(Collider other)
      {
         if (other.gameObject.layer != 6) return;
         _characterIsInside = false;
      }


      private IEnumerator CharacterBuffer()
      {
         yield return new WaitForSeconds(0.1f);
         if (_characterIsInside) StartCoroutine(RespawnPlayer());
      }
   
      IEnumerator RespawnPlayer()
      {
         KeyUI.instance.FadeInBlackScreen(0.5f);
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
    }
}
