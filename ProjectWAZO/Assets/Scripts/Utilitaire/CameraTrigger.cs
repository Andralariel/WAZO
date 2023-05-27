using System.Collections.Generic;
using _3C;
using UnityEngine;

namespace Utilitaire
{
   public class CameraTrigger : MonoBehaviour
   {
      public enum Effect
      {
         GoTopDown,
         GoIso,
         GoVerticalLerp,
         Zoom,
         ChangeOffest,
         OnePointOffset,
         FocusOn,
         StartCinématique,
      }

      public bool doOnce;
      private CameraController camera;
      public List<GameObject> objectsToKill;
      public Effect cameraEffect;
      public Vector3 newOffset;
      public float newSmoothFactor = 0.5f;
      private float oldSmoothFactor;
  
      [Header("OnePointOffest")] 
      public Vector3 originalOffset;
      public bool fixColline;
  
      [Header("Focus")] 
      public GameObject objectToFocus;

      [Header("Lerp Options")] 
      public GameObject newTarget1;
      public GameObject newTarget2;
      public Vector3 newLerpGoal;

      [Header("StartCinématique")]
      public int cinématiqueToStart;

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
                  originalOffset = camera.offset;
                  camera.offset = newOffset;
                  oldSmoothFactor = camera.SmoothMoveFactor;
                  camera.SmoothMoveFactor = newSmoothFactor;
                  break;
               case Effect.Zoom:
                  camera.Zoom(0.5f);
                  break;
               case Effect.ChangeOffest:
                  camera.offset = newOffset;
                  break;
               case Effect.OnePointOffset:
                  originalOffset = camera.offset;
                  camera.offset = newOffset;
                  oldSmoothFactor = camera.SmoothMoveFactor;
                  camera.SmoothMoveFactor = newSmoothFactor;
                  break;
               case Effect.StartCinématique:
                  CinématiqueManager.instance.StartCinématique(cinématiqueToStart);
                  break;
            }
            
            foreach (GameObject obj in objectsToKill)
            {
               obj.GetComponent<BoxCollider>().enabled = false;
            }
            if (doOnce)
            {
               Destroy(gameObject);
            }
         }
        
      }

      private void OnTriggerExit(Collider other)
      {
         if (other.gameObject.layer == 6)
         {
            switch (cameraEffect)
            {
               case Effect.OnePointOffset:
                  if (!fixColline)
                  {
                     camera.offset = originalOffset;
                     //camera.SmoothMoveFactor = oldSmoothFactor;
                     CameraController.instance.filmPlayer = false;
                  }
                  else
                  {
                     camera.offset = new Vector3(2, 10, -8.5f);
                     //camera.SmoothMoveFactor = 0.2f;
                     CameraController.instance.filmPlayer = false;
                  }
             
                  originalOffset = Vector3.zero;
                  break;
               case Effect.FocusOn:
                  camera.isFocused = false;
                  camera.isIso = true;
                  camera.offset = originalOffset;
                  camera.focusedObject = null;
                  CameraController.instance.filmPlayer = false;
                  break;
            }
         }
      }
   }
}
