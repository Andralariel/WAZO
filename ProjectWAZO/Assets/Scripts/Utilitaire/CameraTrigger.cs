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
     GoVerticalLerp,
     Zoom,
     ChangeOffest,
     FocusOn,
     StartCinématique,
  }

  public bool doOnce;
  private CameraController camera;
  public List<GameObject> objectsToKill;
  public Effect cameraEffect;
  public Vector3 newOffset;
  
  [Header("Focus")] 
  public GameObject objectToFocus;

  [Header("Lerp Options")] 
  public GameObject newTarget1;
  public GameObject newTarget2;
  public Vector3 newLerpGoal;

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
               camera.isVerticalLerp = false;
               break;
            case Effect.GoIso:
               camera.isIso = true;
               camera.isTopDown = false;
               camera.isFocused = false;
               camera.isVerticalLerp = false;
               break;
            case Effect.GoVerticalLerp:
               camera.isIso = false;
               camera.isTopDown = false;
               camera.isFocused = false;
               camera.isVerticalLerp = true;
               camera.target1 = newTarget1;
               camera.target2 = newTarget2;
               camera.lerpGoal = newLerpGoal;
               camera.SavePosition();
               break;
            case Effect.FocusOn:
               camera.isFocused = true;
               camera.isTopDown = false;
               camera.isIso = false;
               camera.isVerticalLerp = false;
               camera.focusedObject = objectToFocus.transform;
               break;
            case Effect.Zoom:
               camera.Zoom(0.5f);
               break;
            case Effect.ChangeOffest:
               camera.offset = newOffset;
               break;
            case Effect.StartCinématique:
              StartCoroutine( CinématiqueManager.instance.CinématiqueBOTW());
               break;
         }
      }

      if (doOnce)
      {
         foreach (GameObject obj in objectsToKill)
         {
            obj.GetComponent<BoxCollider>().enabled = false;
         }
      }
   }
}
