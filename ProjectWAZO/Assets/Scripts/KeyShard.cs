using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class KeyShard : MonoBehaviour
{
   public AnimationCurve curve;
   public bool isAnimation;
   public float force;
   public Vector3 ralentissement = Vector3.zero;
   public float timeToGoBack;
   public float t;
   private Rigidbody rb;

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   private void Update()
   {
      if (isAnimation)
      {
       t += timeToGoBack * Time.deltaTime;
      }
      /*else 
      {
         rb.velocity -= ralentissement*Time.deltaTime;
      }*/
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         if (!isAnimation)
         {
            FleePlayer();   
         }
         else
         {
            PickUp();
         }
         
      }
   }

   void FleePlayer()
   {
      Vector3 randomForce = new Vector3(Random.Range(-2f, 2f), 1, Random.Range(-2f, 2f));
      rb.AddForce(randomForce * force,ForceMode.Impulse);
      StartCoroutine(GoBackToPlayer());
     
   }

   void PickUp()
   {
      KeyUI.instance.currentShard += 1;
      KeyUI.instance.ShowKey();
      Destroy(gameObject);
      TempleOpener.instance.currentAmount += 1;
   }

   IEnumerator GoBackToPlayer()
   {
      yield return new WaitForSeconds(0.2f);
      isAnimation = true;
      Vector3 angle = Controller.instance.transform.position - transform.position;
      transform.DOMove(Controller.instance.transform.position, timeToGoBack).SetEase(Ease.InQuart);
      rb.velocity = Vector3.zero;
   }
}
