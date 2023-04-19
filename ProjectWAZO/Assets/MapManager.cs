using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    
    public static MapManager instance;
    public bool MapGot;
    public bool isMenuOpened;
    public bool isRecto;
    public CanvasGroup MapMenu;
    public GameObject mapElements;
    public GameObject loreElements;
    public RectTransform iconMapUpdate;
    public Image Map;
    public Sprite mapVierge;
    public Sprite mapPleine;
    public List<Image> listCroix;
    public List<Image> doneFilterList;
    public List<Image> fresquesList;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Map.sprite = mapVierge;
    }

    public void OpenCloseMap()
    {
        if (Controller.instance.isGrounded && !CinématiqueManager.instance.isCinématique)
        {
            if (isMenuOpened)
            {
                isMenuOpened = false;
                MapMenu.DOFade(0, 0.5f);
                Controller.instance.canJump = true;
                Controller.instance.canMove = true;   
                KeyUI.instance.HideMapKey();
                
                foreach (Image img in doneFilterList)
                {
                    img.DOFade(0, 0.5f);
                }
            }
            else
            {
                isMenuOpened = true;
                MapMenu.DOFade(1, 0.5f);
                Controller.instance.canJump = false;
                Controller.instance.canMove = false;  
                KeyUI.instance.ShowMapKey();

                if (KeyUI.instance.keyInRegion["Village"] == 0) // Met les zones en couleur si toutes les clés sont récupérées
                {
                    doneFilterList[0].DOFade(1, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Bosquet"] == 0)
                {
                    doneFilterList[1].DOFade(1, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Hameau"] == 0)
                {
                    doneFilterList[2].DOFade(1, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Plaine"] == 0)
                {
                    doneFilterList[3].DOFade(1, 1.2f);
                }
                
                
                if (!isRecto) // Retourne la feuille pour afficher la carte
                {
                    isRecto = true;
                    Map.transform.DOScaleX(0, 0.1f);
                    if (MapGot)
                    {
                        Map.sprite = mapPleine;
                    }
                    else
                    {
                        Map.sprite = mapVierge;
                    }
                   
                    mapElements.SetActive(true);
                    loreElements.SetActive(false);
                    Map.transform.DOScaleX(1, 0.1f);
                }
            }
        }
    }

    public void ReturnMap() // Retourner la feuille entre carte et lore
    {
        if (isMenuOpened)
        {
            if (isRecto)
            {
                isRecto = false;
                Map.transform.DOScaleX(0, 0.5f);
                mapElements.SetActive(false);
                loreElements.SetActive(true);
                Map.sprite = mapVierge;
                Map.transform.DOScaleX(-1, 0.5f);
            }
            else
            {
                isRecto = true;
                Map.transform.DOScaleX(0, 0.5f);
                if (MapGot)
                {
                    Map.sprite = mapPleine;  
                }
                else
                {
                    Map.sprite = mapVierge;
                }
                mapElements.SetActive(true);
                loreElements.SetActive(false);
                Map.transform.DOScaleX(1, 0.5f);
            }
        }
    }

    public void IconMapUpdate(float duration)
    {
        StartCoroutine(ShowAndHide(duration));
    }

    IEnumerator ShowAndHide(float duration)
    {
        iconMapUpdate.gameObject.SetActive(true);
        iconMapUpdate.DOAnchorPosX(0, 0.5f);
        yield return new WaitForSeconds(duration);
        iconMapUpdate.DOAnchorPosX(200, 0.5f).OnComplete((() =>   iconMapUpdate.gameObject.SetActive(false)));
      
    }
}
