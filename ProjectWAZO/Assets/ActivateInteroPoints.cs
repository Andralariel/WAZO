using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInteroPoints : MonoBehaviour
{
   public int ID;
   public bool isFresque;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      { 
         if(isFresque)
         {
            MapManager.instance.pontInteroFresques[ID-1].gameObject.SetActive(true);
            Destroy(this);
         }
         else
         {
            MapManager.instance.pontIntero[ID-1].gameObject.SetActive(true);
            Destroy(this);
         }
      }
   }
}
