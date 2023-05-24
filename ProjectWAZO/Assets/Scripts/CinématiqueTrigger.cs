using System.Collections;
using _3C;
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

  /* private void Update()
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
         player.ChangeAnimSpeed(1);
         player.anim.SetBool("isWalking",false);
         player.anim.SetBool("isIdle",true);
         isMoving = false;
         var lookPos = thingToLook.transform.position - PointToGo.position;
         lookPos.y = 0;
         var rotation = Quaternion.LookRotation(lookPos);
         player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed);
      }
   }*/
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
         EndedMoving = true;
         CinématiqueManager.instance.isCinématique = false;
         player.canMove = false;
         CameraController.instance.SmoothMoveFactor = 0.8f;
         player.ChangeAnimSpeed(0.3f);
         player.anim.SetBool("isWalking",true);
         player.anim.SetBool("isIdle",false);
         player.canJump = false;
         Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
         player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => StartCoroutine(OpenMenu())));
         MapManager.instance.UnlockFresque(FresqueID);
      }
   }
   
  /* private void OnTriggerEnter(Collider other)
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
   }*/
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
