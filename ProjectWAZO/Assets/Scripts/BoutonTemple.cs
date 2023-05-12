using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonTemple : MonoBehaviour
{
   public TempleManager temple;
   public int boutonIndex;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         if (boutonIndex == 1)
         {
            temple.ActivateEscalier1();
            this.enabled = false;
         }
         else if (boutonIndex == 2)
         {
            temple.ActivateEscalier2();
         }
        
      }
   }
}
