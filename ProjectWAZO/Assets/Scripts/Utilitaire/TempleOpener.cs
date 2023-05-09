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
   public Animator animDoor;
   public GameObject Serrure;
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

      if (other.gameObject.layer == 6)
      {
         KeyUI.instance.ShowMapKey();
      }
   }
   
   public void OnTriggerExit(Collider other)
   {
      if (other.gameObject.layer == 6)
      {
         KeyUI.instance.HideMapKey();
      }
   }

   IEnumerator CinémtiqueOuverture()
   {
      CinématiqueManager.instance.isCinématique = true;
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
         keyShardCinématique[i].SetActive(true);
         keyShardCinématique[i].transform.DOMove(emptyPosition[i].transform.position, 3f);
      }
      yield return new WaitForSeconds(3.5f);
      for (int i = 0; i < 10; i++)
      {
         keyShardCinématique[i].transform.DOMove(Serrure.transform.position, 5f);
      }
      yield return new WaitForSeconds(5f);
      for (int i = 0; i < 10; i++)
      {
         Destroy(keyShardCinématique[i]);
      }
      yield return new WaitForSeconds(0.5f);
      animDoor.SetBool("Open",true);
      CameraController.instance.transform.DOShakePosition(5, 1, 11);
      yield return new WaitForSeconds(5f);
      CinématiqueManager.instance.isCinématique = false;
      Controller.instance.canMove = true;
      Controller.instance.canJump = true;
      CameraController.instance.canMove = true;
      Destroy(gameObject);
   }
}
