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
     Zoom,
     ChangeOffest,
     FocusOn,
  }
  private CameraController camera;
  public Effect cameraEffect;
  public Vector3 newOffset;
  public GameObject objectToFocus;

  //----------------------------------------------------------------------------------------------

  private void Start()
  {
     camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
  }

  private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         switch (cameraEffect)
         {
            case Effect.GoTopDown:
               camera.isTopDown = true;
               camera.isIso = false;
               camera.isFocused = false;
               break;
            case Effect.GoIso:
               camera.isIso = true;
               camera.isTopDown = false;
               camera.isFocused = false;
               break;
            case Effect.FocusOn:
               camera.isFocused = true;
               camera.isTopDown = false;
               camera.isIso = false;
               camera.focusedObject = objectToFocus.transform;
               break;
            case Effect.Zoom:
               camera.Zoom(0.5f);
               break;
            case Effect.ChangeOffest:
               camera.offset = newOffset;
               break;
         }
      }
   }
}
