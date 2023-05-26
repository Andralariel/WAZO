using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using Sound;
using TechArt;
using UnityEngine;
using UnityEngine.VFX;
using Utilitaire;
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
   [SerializeField] private VisualEffect beacon;

   private bool ispickedup;
   public enum Region
   {
      Village,
      Bosquet,
      Hameau,
      Plaine,
      Cimetière,
   }

   public Region choseRegion;
 

   private void Start()
   {
      ispickedup = false;
      vfxidle.Play();
      rb = GetComponent<Rigidbody>();
   }
   
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
      beacon.Stop();
      AudioList.Instance.PlayOneShot(AudioList.Instance.getKey, AudioList.Instance.getKeyVolume);
      if (!ispickedup) vfxpickup.Play();
      ispickedup = true;
      Debug.Log("1");
      for (int i = 0; i < spiritsToKill.Count; i++)
      {
         Destroy(spiritsToKill[i]);
      }
      if (KeyUI.instance.currentShard <= 6)
      {
         Debug.Log("Clé Normale");
         KeyUI.instance.ShowKey();
         TempleOpener.instance.currentAmount += 1;
         KeyUI.instance.currentShard += 1;
         TempleOpener.instance.CheckKeyState();
      }
      
      if (KeyUI.instance.currentShard >= 7)
      {
         Debug.Log("Clé Bonus");
         KeyUI.instance.ShowAdditionalKey();
         TempleOpener.instance.CheckKeyState();
         KeyUI.instance.currentBonusShard += 1;
      }
     
      Destroy(gameObject);
      
   }

  
}
