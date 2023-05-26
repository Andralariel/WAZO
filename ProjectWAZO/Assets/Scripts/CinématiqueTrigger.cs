using System;
using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;

public class CinématiqueTrigger : MonoBehaviour
{
   public Controller player;
   public GameObject thingToLook;
   public Transform PointToGo;
   public bool EndedMoving;
   public float CinématiqueDuration;
   public float timeToGo;
   public float RotateSpeed;
   public int FresqueID;
   public bool repetable;

   private void Update()
  {
     if (EndedMoving)
     {
        Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
        var rotation = Quaternion.LookRotation(pointToGo);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed);
     }
  }
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         //EndedMoving = true;
         player.canJump = false;
         player.canMove = false;
         Vector2 angle =  PointToGo.position - player.transform.position ;
         player.moveInput = angle.normalized;
         CinématiqueManager.instance.isCinématique = false;
         CameraController.instance.SmoothMoveFactor = 0.8f;
         //CameraController.instance.player = PointToGo.gameObject;
         //CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*3, 5f);
         player.ChangeAnimSpeed(0.3f);
         player.anim.SetBool("isWalking",true);
         player.anim.SetBool("isIdle",false);
         Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
         player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => StartCoroutine(OpenMenu())));
      }
   }
   
   IEnumerator OpenMenu()
   {
      EndedMoving = true;
      CameraController.instance.SmoothMoveFactor = 0.2f;
      yield return new WaitForSeconds(CinématiqueDuration);
      NarrationMenuManager.instance.ChangeFresque(FresqueID);
      NarrationMenuManager.instance.OpenMenu();
      MapManager.instance.UnlockFresque(FresqueID);
      
      //Fix rotation
      Controller.instance.transform.rotation = Quaternion.Euler(Vector3.zero);
      
      if (!repetable)
      {
         Destroy(gameObject);
      }
   }
}
