using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
  public enum Effect
  {
     GoTopDown,
     GoIso,
     Zoom
  }
  public CameraController camera;
  public Effect cameraEffect;

  //----------------------------------------------------------------------------------------------
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         switch (cameraEffect)
         {
            case Effect.GoTopDown:
               camera.isTopDown = true;
               break;
            case Effect.GoIso:
               camera.isTopDown = false;
               break;
            case Effect.Zoom:
               camera.Zoom(0.5f);
               break;
         }
      }
   }
}
