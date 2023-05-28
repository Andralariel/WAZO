using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilitaire
{
   public class ChangeSceneTrigger : MonoBehaviour
   {
      public Image blackScreen;
      private void OnTriggerEnter(Collider other)
      {
         if (other.gameObject.layer == 6) //Si c'est le player
         {
            CameraController.instance.canMove = false;
            Controller.instance.canMove = false;
            Controller.instance.canJump = false;
            Controller.instance.transform.DOMove(new Vector3(Controller.instance.transform.position.x,
               Controller.instance.transform.position.y, Controller.instance.transform.position.z+50),10);
            StartCoroutine(ChangeScene());
         }
      }

      IEnumerator ChangeScene()
      {
         CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*20, 5f); 
         yield return new WaitForSeconds(1.2f);
         blackScreen.DOFade(1, 0.8f);
         yield return new WaitForSeconds(1f);
         SceneManager.LoadScene("Temple Test");
      }
   }
}
