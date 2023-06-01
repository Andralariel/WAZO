
using System;
using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using UnityEngine;

public class EboulementManager : MonoBehaviour
{
   //public int eventIndex;
   public bool isDouble;
   public float timeBeforeEvent1;
   public float timeBeforeEvent2;

   [Header("CamShake")] 
   public float duration;
   public float force;
   public int vibrato;
   
   [Header("Event Obj1")] 
   public GameObject ObjToMove;
   public ParticleSystem vfx1;
   public Vector3 rotationToGo;
   public float timeToWait;
   public Vector3 positionToGo;

   [Header("Event Obj2")]
   private Collider collider;
   public GameObject ObjToMove2;
   public ParticleSystem vfx2;
   public Vector3 rotationToGo2;
   public float timeToWait2;
   public Vector3 positionToGo2;

   private void Start()
   {
      collider = GetComponent<BoxCollider>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         StartCoroutine(Event1());
         if (isDouble)
         {
            StartCoroutine(Event2());
         }
         CameraController.instance.transform.parent.DOShakePosition(duration,force,vibrato,10);
         collider.enabled = false;
      }
   }

   public IEnumerator Event1()
   {
      yield return new WaitForSeconds(timeBeforeEvent1);
      ObjToMove.transform.DOLocalRotate(rotationToGo, timeToWait);
      ObjToMove.transform.DOLocalMove(positionToGo, timeToWait);
      yield return new WaitForSeconds(timeToWait);
      vfx1.Play();
   }
   
   public IEnumerator Event2()
   {
      yield return new WaitForSeconds(timeBeforeEvent2);
      ObjToMove2.transform.DOLocalRotate(rotationToGo2, timeToWait2);
      ObjToMove2.transform.DOLocalMove(positionToGo2, timeToWait2);
      yield return new WaitForSeconds(timeToWait2);
      vfx2.Play();
   }
   
}
