using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TempleOpener : MonoBehaviour
{
   public float currentAmount;
   public float AmountToOpen;
   public bool canOpen;
   public static TempleOpener instance;
   public List<GameObject> keyShardCinématique;
   public List<GameObject> emptyPosition;
   

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
   }

   public void CheckKeyState()
   {
      if (currentAmount >= AmountToOpen)
      {
         canOpen = true;
      }
   }

   public void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 6 && canOpen)
      {
         StartCoroutine(CinémtiqueOuverture());
      }
   }

   IEnumerator CinémtiqueOuverture()
   {
      Controller.instance.canMove = false;
      Controller.instance.canJump = false;
      CameraController.instance.canMove = false;
      CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*20, 8f); 
      foreach (GameObject obj in keyShardCinématique)
      {
         obj.transform.position = Controller.instance.transform.position;
      }
      for (int i = 0; i < 10; i++)
      {
         keyShardCinématique[i].transform.DOMove(emptyPosition[i].transform.position, 3f);
      }
      yield return new WaitForSeconds(3.5f);
      for (int i = 0; i < 10; i++)
      {
         keyShardCinématique[i].transform.DOMove(transform.position+new Vector3(0,5.3f,0), 5f);
      }
      yield return new WaitForSeconds(5f);
     
      yield return new WaitForSeconds(0.5f);
      transform.DOMove(new Vector3(transform.position.x, transform.position.y - 20, transform.position.z),5f);
      CameraController.instance.transform.DOShakePosition(5, 1, 11);
      yield return new WaitForSeconds(5f);
      for (int i = 0; i < 10; i++)
      {
         Destroy(keyShardCinématique[i]);
      }
      Controller.instance.canMove = true;
      Controller.instance.canJump = true;
      CameraController.instance.canMove = true;
   }
}
