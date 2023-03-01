using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
   public bool turnOn;
   public CameraController camera;

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         if (turnOn)
         {
            camera.isTopDown = true;
         }
         else
         {
            camera.isTopDown = false;
         }

      }
   }
}
