using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendingWind : MonoBehaviour
{
   public float windForce;
   private void OnTriggerStay(Collider other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,windForce,0),ForceMode.Acceleration);
      }
   }
}
