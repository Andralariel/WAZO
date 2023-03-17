using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BigDoorOpener : MonoBehaviour
{
   public float currentAmount;
   public float AmountToOpen;
   public float amountToGo;
   public bool isCinematic;
   public Controller player;
   public TextMeshProUGUI text;
   private CameraController camera;

   private void Start()
   {
      camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
      text.text = currentAmount + " / " + AmountToOpen;
   }

   private void Update()
   {
      if (currentAmount >= AmountToOpen)
      {
         Open();
      }
   }

   public void UpdateText()
   {
      text.text = currentAmount + " / " + AmountToOpen;
   }

   void Open()
   {
      if (isCinematic)
      {
         StartCoroutine(Cinematique());
         isCinematic = false;
      }
   }

   IEnumerator Cinematique()
   {
      player.canMove = false;
      camera.player = gameObject;
      yield return new WaitForSeconds(1.5f);
      UpdateText();
      yield return new WaitForSeconds(1f);
      camera.transform.DOShakePosition(0.2f, 0.1f);
      transform.DOMove(transform.position - new Vector3(0,amountToGo,0), 0.5f);
      yield return new WaitForSeconds(2f);
      player.canMove = true;
      camera.player = player.gameObject;
   }
}
