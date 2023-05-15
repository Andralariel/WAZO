using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilitaire;

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
      if (other.gameObject.layer == 6)
      {
         StartCoroutine(RespawnPlayer());
      }

      if (other.gameObject.layer == 7)
      {
         other.GetComponent<Spirit>()?.Respawn();
      }
   }
   
   
   
   IEnumerator RespawnPlayer()
   {
      KeyUI.instance.FadeInBlackScreen(0.5f);
      Controller.instance.canMove = false;
      Controller.instance.canJump = false;
      yield return new WaitForSeconds(0.6f);
      Controller.instance.transform.position = respawnPoint + new Vector3(0, 2, 0);
      yield return new WaitForSeconds(0.6f);
      Controller.instance.canMove = true;
      Controller.instance.canJump = true;
      KeyUI.instance.FadeOutBlackScreen(0.5f);
   }

}
