using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeightUI : MonoBehaviour
{
   private RectTransform rectTransform;
   private CanvasGroup canvasGroup;
   public CameraController camera;
   public TextMeshProUGUI text;
   public int currentWeight;
   public int maxWeight;
   public bool isVisible;
   
   public enum InteracteurAssocied
   {
      balance,
      altar,
      bouton,
      ascenceur
   }

   public InteracteurAssocied interacteur;
   
   void Start()
   {
       rectTransform = GetComponent<RectTransform>();
       canvasGroup = GetComponent<CanvasGroup>();
   }
   
    void Update()
    {
       rectTransform.LookAt(camera.transform);

       if (isVisible && canvasGroup.alpha < 1)
       {
           canvasGroup.alpha += Time.deltaTime;
       }
       else if(!isVisible && canvasGroup.alpha > 0)
       {
           canvasGroup.alpha -= Time.deltaTime;
       }
       
       if (currentWeight == 0)
       {
         isVisible = false;
       }
       else
       {
         isVisible = true;
       }

       switch (interacteur)
       {
           case InteracteurAssocied.balance:
               text.text = currentWeight + "";
               break;
           
           case InteracteurAssocied.altar:
               text.text = currentWeight + " / " + maxWeight;
               break;
           
           case InteracteurAssocied.bouton:
               text.text = currentWeight + " / " + maxWeight;
               break;
           
           case InteracteurAssocied.ascenceur:
               text.text = currentWeight + "";
               break;
       }
    }
}
