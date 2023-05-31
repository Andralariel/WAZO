using System.Collections;
using _3C;
using Sound;
using TechArt;
using UnityEngine;

public class TriggerWater : MonoBehaviour
{
   private float originalSpeed;
   public float waterSpeed;
   
   [SerializeField] private Collider thisCol;
   
   private ParticleSystem _splash;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6) //6 = player
      {
         Controller.instance.walkMoveSpeed = waterSpeed;

         AudioList.Instance.PlayOneShot(AudioList.Instance.splashPlayer, AudioList.Instance.splashPlayerVolume);
         Splash(other);
      }

      if (other.gameObject.layer == 14) //14 = objects
      {
         if(other.attachedRigidbody.isKinematic) return;
         
         AudioList.Instance.PlayOneShot(AudioList.Instance.splashObject, AudioList.Instance.splashObjectVolume);
         Splash(other);
         StartCoroutine(SinkingObject(other.gameObject));
      }
   }
   
   private void Splash(Collider other)
   {
      var pos = other.transform.position;
      _splash = SplashPoolingSystem.Instance.LendASplash();
      _splash.transform.position = new Vector3(pos.x,thisCol.bounds.max.y+0.5f,pos.z);
      _splash.Play();
   }

   private IEnumerator SinkingObject(GameObject other)
   {
      yield return new WaitForSeconds(0.1f);
      other.SetActive(false);
   }
}
