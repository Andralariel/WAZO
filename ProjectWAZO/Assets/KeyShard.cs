using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyShard : MonoBehaviour
{
   public AnimationCurve curve;
   public float currentMove;

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         CinématiqueClé();
      }
   }

   void CinématiqueClé()
   {
      Destroy(gameObject);
      TempleOpener.instance.currentAmount += 1;
   }
}
