using _3C;
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
   
        void Start()
        {
            camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }
   
        void Update()
        {
            //text.rectTransform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
            //text.rectTransform.rotation = new Quaternion(50,text.rectTransform.rotation.y,0,0);
            //text.rectTransform.LookAt(camera.transform);

            if (PauseMenu.instance.enableText)
            {
                if (isVisible && canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += Time.deltaTime;
                }
                else if(!isVisible && canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha -= Time.deltaTime;
                }
            }
            else
            {
                canvasGroup.alpha = 0;
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
}
