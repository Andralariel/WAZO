
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EboulementManager : MonoBehaviour
{
   //public int eventIndex;

   [Header("Event Obj1")] 
   public GameObject ObjToMove;
   public Vector3 rotationToGo;
   public float timeToRotate;
   public float timeToWait;
   public Vector3 positionToGo;
   public float timeToMove;
   
   [Header("Event Obj2")] 
   public GameObject ObjToMove2;
   public Vector3 rotationToGo2;
   public float timeToRotate2;
   public float timeToWait2;
   public Vector3 positionToGo2;
   public float timeToMove2;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         Debug.Log("éboulement");
         StartCoroutine(Event1());
         StartCoroutine(Event2());
      }
   }

   public IEnumerator Event1()
   {
      ObjToMove.transform.DOLocalRotate(rotationToGo, timeToRotate);
      yield return new WaitForSeconds(timeToWait);
      ObjToMove.transform.DOLocalMove(positionToGo, timeToMove);
   }
   
   public IEnumerator Event2()
   {
      ObjToMove2.transform.DOLocalRotate(rotationToGo2, timeToRotate2);
      yield return new WaitForSeconds(timeToWait2);
      ObjToMove2.transform.DOLocalMove(positionToGo2, timeToMove2);
   }
   
}
