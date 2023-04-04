using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
      KeyUI.instance.currentShard += 1;
      KeyUI.instance.Show();
      Destroy(gameObject);
      TempleOpener.instance.currentAmount += 1;
   }
}
