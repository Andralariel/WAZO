using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendingWind : MonoBehaviour
{
   public float windForce;
   public int MassFactor;
   
   private void OnTriggerStay(Collider other)
   {
      if (other.gameObject.CompareTag("Player") && Controller.instance.isPressing)
      {
            Controller.instance.gravityScale = -4;
            Controller.instance.canJump = false;
            other.gameObject.GetComponent<Controller>().isWind = true;
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,windForce - other.attachedRigidbody.mass*MassFactor,0),ForceMode.Acceleration);
      }
   }
   
   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         other.gameObject.GetComponent<Controller>().isWind = false;
      }
   }
}
