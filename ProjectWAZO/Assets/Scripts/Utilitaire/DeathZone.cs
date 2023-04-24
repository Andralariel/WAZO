using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilitaire;

public class DeathZone : MonoBehaviour
{
   public Vector3 respawnPoint;

   public void Update()
   {
      if (Controller.instance.isGrounded)
      {
         respawnPoint = Controller.instance.transform.position;
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         other.transform.position = respawnPoint + new Vector3(0, 2, 0);
      }

      if (other.gameObject.layer == 7)
      {
         other.GetComponent<Spirit>()?.Respawn();
      }
   }
}
