using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utilitaire
{
   public class TempleOpener : MonoBehaviour
   {
      public float currentAmount;
      public float AmountToOpen;
      public bool canOpen;
      public static TempleOpener instance;
      public Animator animDoor;
      public GameObject Serrure;
      public GameObject Clé;
      public GameObject rassemblementClé;
      public Image whiteScreen;
      public List<GameObject> keyShardCinématique;
      public List<GameObject> emptyPosition;
      public BoxCollider colliderPorte;
   

      private void Awake()
      {
         if (instance == null)
         {
            instance = this;
         }
      }

      public void CheckKeyState()
      {
         if (currentAmount >= AmountToOpen)
         {
            canOpen = true;
         }
      }

      public void OnTriggerEnter(Collider other)
      {
         if (other.gameObject.layer == 6 && canOpen)
         {
            StartCoroutine(CinémtiqueOuverture());
         }

         if (other.gameObject.layer == 6)
         {
            KeyUI.instance.ShowMapKey();
         }
      }
   
      public void OnTriggerExit(Collider other)
      {
         if (other.gameObject.layer == 6)
         {
            KeyUI.instance.HideMapKey();
         }
      }

      IEnumerator CinémtiqueOuverture()
      {
         CinématiqueManager.instance.isCinématique = true;
         CarnetManager.instance.canOpen = false;
         Controller.instance.anim.SetBool("isWalking",false);
         Controller.instance.anim.SetBool("isIdle",true);
         Controller.instance.canMove = false;
         Controller.instance.canJump = false;
         Controller.instance.ultraBlock = true;
         CameraController.instance.canMove = false;
         CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*18, 7f); 
         yield return new WaitForSeconds(3f);
         Controller.instance.anim.SetBool("isIdle",true);
         foreach (GameObject obj in keyShardCinématique)
         {
            obj.transform.position = Controller.instance.transform.position;
         }
         for (int i = 0; i < 6; i++)
         {
            keyShardCinématique[i].SetActive(true);
            keyShardCinématique[i].transform.DOMove(emptyPosition[i].transform.position, 3f);
         }
         yield return new WaitForSeconds(3.5f);
         for (int i = 0; i < 6; i++)
         {
            keyShardCinématique[i].transform.DOMove(rassemblementClé.transform.position, 3f);
         }
         yield return new WaitForSeconds(3f);
         whiteScreen.DOFade(1, 0.3f);
         yield return new WaitForSeconds(0.3f);
         Clé.SetActive(true);
         yield return new WaitForSeconds(0.3f);
         whiteScreen.DOFade(0, 0.3f);
         for (int i = 0; i < 6; i++)
         {
            Destroy(keyShardCinématique[i]);
         }
         yield return new WaitForSeconds(3f);
         Clé.transform.DOMove(Serrure.transform.position, 5f);
         yield return new WaitForSeconds(4.5f);
         Clé.transform.DOLocalRotate(new Vector3(0,90,-45), 0.5f);
         yield return new WaitForSeconds(0.5f);
         animDoor.SetBool("Open",true);
         CameraController.instance.transform.DOShakePosition(5, 1, 11);
         yield return new WaitForSeconds(0.5f);
         Clé.transform.DOMove(Clé.transform.position - new Vector3(0,10,0), 5f);
         yield return new WaitForSeconds(4.5f);
         CinématiqueManager.instance.isCinématique = false;
         Controller.instance.canMove = true;
         Controller.instance.canJump = true;
         Controller.instance.ultraBlock = false;
         CameraController.instance.canMove = true;
         colliderPorte.enabled = false;
         Destroy(gameObject);
      }
   }
}
