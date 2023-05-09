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
   public float TimeToGoBack;
   private int goBackAtempts;
   public List<GameObject> spiritsToKill;
   private Rigidbody rb;
   [SerializeField] private ParticleSystem vfxpickup;
   [SerializeField] private ParticleSystem vfxidle;

   private bool ispickedup;
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
      ispickedup = false;
      vfxidle.Play();
      rb = GetComponent<Rigidbody>();
   }

   /*private void Update()
   {

      if (isGoingBack)
      {
         ForceToGoBack += 0.2f;
         Vector3 angle = Controller.instance.transform.position - transform.position;
         rb.AddForce(angle*ForceToGoBack);
         //transform.DOLocalMove(Vector2.zero, ForceToGoBack);
      }
   }*/

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         if (!isGoingBack)
         {
            //transform.parent = other.transform;
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
      StartCoroutine(WaitToGoBack());
     
   }
   
   IEnumerator WaitToGoBack()
   {
      yield return new WaitForSeconds(0.5f);
      isGoingBack = true;
      GoToPlayer();
   }

   void GoToPlayer()
   {
      transform.DOMove(Controller.instance.transform.position, TimeToGoBack-(goBackAtempts*0.1f)).SetEase(Ease.Linear).OnComplete((() => CheckDone()));
   }

   void CheckDone()
   {
      goBackAtempts++;
      GoToPlayer();
   }
   
   void PickUp()
   {
      vfxidle.Stop();
      if (!ispickedup) vfxpickup.Play();
      ispickedup = true;
      Debug.Log("1");
      for (int i = 0; i < spiritsToKill.Count; i++)
      {
         Destroy(spiritsToKill[i]);
      }
      KeyUI.instance.currentShard += 1;
      TempleOpener.instance.currentAmount += 1;
      TempleOpener.instance.CheckKeyState();
      KeyUI.instance.ShowKey();
      Destroy(gameObject);
      
   }

  
}
