using System;
using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;

public class CinématiqueTrigger : MonoBehaviour
{
   public GameObject fresque;
   public Transform PointToGo;
   public float CinématiqueDuration;
   public int FresqueID;
   public bool repetable;
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         Controller.instance.pointToGo = PointToGo.gameObject;
         Controller.instance.thingToLook = fresque;
         Controller.instance.cineSpeed = 0.8f;
         Controller.instance.isGoing = true;
         Controller.instance.canMove = false;
         Controller.instance.canJump = false;
         StartCoroutine(OpenMenu());
      }
   }
   
   IEnumerator OpenMenu()
   {
      MapManager.instance.pontInteroFresques[FresqueID].gameObject.SetActive(false);
      MapManager.instance.listCercles[FresqueID].gameObject.SetActive(false);
      CarnetManager.instance.canOpen = false;
      CameraController.instance.SmoothMoveFactor = 0.2f;
      yield return new WaitForSeconds(CinématiqueDuration);
      NarrationMenuManager.instance.ChangeFresque(FresqueID);
      NarrationMenuManager.instance.OpenMenu();
      MapManager.instance.isFresqueUnlocked[FresqueID] = true;
     
      
      //Fix rotation
      Controller.instance.transform.rotation = Quaternion.Euler(Vector3.zero);
      
      if (!repetable)
      {
         Destroy(gameObject);
      }
   }
   
   //-------------------------Archives----------------------------------------------------------
   
   /*player.canJump = false;
         
         Vector2 angle =  PointToGo.position - player.transform.position ;
         Debug.Log(angle.normalized);
         player.moveInput = angle.normalized;
         CinématiqueManager.instance.isCinématique = false;
         CameraController.instance.SmoothMoveFactor = 0.8f;
         //CameraController.instance.player = PointToGo.gameObject;
         //CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*3, 5f);
         player.ChangeAnimSpeed(0.3f);
         player.anim.SetBool("isWalking",true);
         player.anim.SetBool("isIdle",false);
         Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
         //player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => StartCoroutine(OpenMenu())));*/
}
