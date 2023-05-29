using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoTrigger : MonoBehaviour
{
    public bool DoOnce;
    public bool isMap;
    public GameObject ObjToActive;
    public List<GameObject> tutoSectionsList;
    public CanvasGroup CanvasG;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            for (int i = 0; i < tutoSectionsList.Count; i++)
            {
                tutoSectionsList[i].SetActive(false);
            }
            ObjToActive.SetActive(true);
            CanvasG.DOFade(1, 1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (CarnetManager.instance.isOpened && isMap)
            {
                CanvasG.DOFade(0, 1.5f).OnComplete((() => Destroy(gameObject)));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            CanvasG.DOFade(0, 1f);
            
            if (DoOnce)
            {
                Destroy(gameObject);
            }
        }

        
    }
}
