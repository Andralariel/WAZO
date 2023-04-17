using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeMoulin : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         other.transform.parent = transform;
      }
   }
   
   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         other.transform.parent = null;
      }
   }
}
