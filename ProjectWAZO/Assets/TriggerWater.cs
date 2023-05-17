using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWater : MonoBehaviour
{
   private float originalSpeed;
   public float waterSpeed;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         originalSpeed = Controller.instance.moveSpeed; 
         Controller.instance.moveSpeed = waterSpeed;
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         Controller.instance.moveSpeed = originalSpeed;
      }   
   }
}
