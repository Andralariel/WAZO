using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInteroPoints : MonoBehaviour
{
   public int ID;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         MapManager.instance.pontIntero[ID-1].gameObject.SetActive(true);
      }
   }
}
