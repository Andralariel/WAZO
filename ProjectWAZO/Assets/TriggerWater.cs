using System;
using System.Collections;
using System.Collections.Generic;
using _3C;
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
         originalSpeed = Controller.instance.walkMoveSpeed; 
         Controller.instance.walkMoveSpeed = waterSpeed;

         Splash(other);
      }

      if (other.gameObject.layer == 14) //14 = objects
      {
         Splash(other);
         StartCoroutine(SinkingObject(other.gameObject));
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         Controller.instance.walkMoveSpeed = originalSpeed;
      }   
   }

   private void Splash(Collider other)
   {
      var pos = other.transform.position;
      _splash = SplashPoolingSystem.Instance.LendASplash();
      _splash.transform.position = new Vector3(pos.x,thisCol.bounds.max.y,pos.z);
      _splash.Play();
   }

   private IEnumerator SinkingObject(GameObject other)
   {
      yield return new WaitForSeconds(0.1f);
      other.SetActive(false);
   }
}
