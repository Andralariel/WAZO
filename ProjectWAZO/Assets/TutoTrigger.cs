using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoTrigger : MonoBehaviour
{
    public bool DoOnce;
    public bool isMap;
    public string textToDisplay;
    public TextMeshProUGUI text;
    public Image ImageOBJ;
    public CanvasGroup CanvasG;
    public Sprite imageVISU; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            text.text = textToDisplay;
            ImageOBJ.sprite = imageVISU;
            CanvasG.DOFade(1, 1.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log("isOnTrigger");
            if (CarnetManager.instance.isOpened)
            {
                Debug.Log("carteouverte");
                CanvasG.DOFade(0, 1.5f).OnComplete((() => Destroy(gameObject)));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            CanvasG.DOFade(0, 1.5f);
            
            if (DoOnce)
            {
                Destroy(gameObject);
            }
        }

        
    }
}
