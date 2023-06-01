using _3C;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WeightSystem
{
    public class WeightUI : MonoBehaviour
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private CameraController camera;
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
   
        private void Start()
        {
            camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void SetMaxAmount(int maxAmount)
        {
            maxWeight = maxAmount;
        }
        
        public void UpdateUI(int currentAmount)
        {
            currentWeight = currentAmount;
            
            //text.rectTransform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
            //text.rectTransform.rotation = new Quaternion(50,text.rectTransform.rotation.y,0,0);
            //text.rectTransform.LookAt(camera.transform);
            
            if (currentWeight == 0)
            {
                isVisible = false;
            }
            else
            {
                isVisible = true;
            }
            
            if (PauseMenu.instance.enableText)
            {
                if (isVisible)
                {
                    canvasGroup.DOFade(1, 0.5f);
                }
                else canvasGroup.DOFade(0, 0.5f);
            }
            else
            {
                canvasGroup.alpha = 0;
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
}
