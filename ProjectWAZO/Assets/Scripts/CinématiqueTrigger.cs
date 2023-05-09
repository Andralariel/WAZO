using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;

public class CinématiqueTrigger : MonoBehaviour
{
   public Controller player;
   public GameObject thingToLook;
   public Transform PointToGo;
   public  bool isMoving;
   public bool EndedMoving;
   public float CinématiqueDuration;
   public float timeToGo;
   public float RotateSpeed;
   public int FresqueID;
   public bool repetable;

   private void Update()
   {
      if (isMoving)
      {
         var lookPos = PointToGo.position - player.transform.position;
         lookPos.y = 0;
         var rotation = Quaternion.LookRotation(lookPos);
         player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed*5);
      }
      
      if (EndedMoving)
      {
         isMoving = false;
         var lookPos = thingToLook.transform.position - PointToGo.position;
         lookPos.y = 0;
         var rotation = Quaternion.LookRotation(lookPos);
         player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed);
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         CinématiqueManager.instance.isCinématique = true;
         isMoving = true;
         player.canMove = false;
         player.canJump = false;
         Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
         player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => EndedMoving = true));
         StartCoroutine(OpenMenu());
         MapManager.instance.fresquesList[FresqueID].DOFade(1, 0.5f);
      }
   }
   IEnumerator OpenMenu()
   {
      yield return new WaitForSeconds(CinématiqueDuration);
      NarrationMenuManager.instance.ChangeFresque(FresqueID);
      NarrationMenuManager.instance.OpenMenu();
      isMoving = false;
      EndedMoving = false;
      if (!repetable)
      {
         Destroy(gameObject);
      }
   }
}
