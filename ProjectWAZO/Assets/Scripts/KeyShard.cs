using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class KeyShard : MonoBehaviour
{
   public int keyID;
   public bool isFleeing;
   public bool isGoingBack;
   public float ForceToFlee;
   public float ForceToGoBack;
   private Rigidbody rb;

   public enum Region
   {
      Village,
      Bosquet,
      Hameau,
      Plaine,
   }

   public Region choseRegion;
 

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   private void Update()
   {

      if (isGoingBack)
      {
         ForceToGoBack += 0.2f;
         Vector3 angle = Controller.instance.transform.position - transform.position;
         rb.AddForce(angle*ForceToGoBack);
         //transform.DOMove(Controller.instance.transform.position, timeToGoBack).SetEase(Ease.InQuart);
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         if (!isGoingBack)
         {
            FleePlayer();   
         }
         else
         {
            PickUp();
            KeyUI.instance.RegisterKey(keyID);
         }
         
      }
   }

   void FleePlayer()
   {
      isFleeing = true;
      Vector3 randomForce = new Vector3(Random.Range(-2f, 2f), 1, Random.Range(-2f,2f));
      rb.AddForce(randomForce * ForceToFlee,ForceMode.Impulse);
      StartCoroutine(GoBackToPlayer());
     
   }
   
   IEnumerator GoBackToPlayer()
   {
      yield return new WaitForSeconds(1f);
      isFleeing = false;
      isGoingBack = true;
      rb.velocity = Vector3.zero;
   }

   void PickUp()
   {
      KeyUI.instance.currentShard += 1;
      TempleOpener.instance.CheckKeyState();
      KeyUI.instance.ShowKey();
      Destroy(gameObject);
      TempleOpener.instance.currentAmount += 1;
   }

  
}
