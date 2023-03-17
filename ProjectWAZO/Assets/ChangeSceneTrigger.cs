using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6) //Si c'est le player
      {
         CameraController.instance.player = gameObject;
         Controller.instance.canMove = false;
         Controller.instance.rb.velocity += (new Vector3(0,0,1) * (65 * Time.deltaTime));
         StartCoroutine(ChangeScene());
      }
   }

   IEnumerator ChangeScene()
   {
      yield return new WaitForSeconds(2f);
      CameraController.instance.transform.DOMove()
      SceneManager.LoadScene("SceneGDPoC");
   }
}
