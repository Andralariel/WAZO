using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CarnetManager : MonoBehaviour
{
    public static CarnetManager instance;
    public List<GameObject> pagesList;
    public CanvasGroup carnet;

    [Header("Float et Int")] 
    public int openedPage;
    public int maxPages;
    public float changePageBuffer;
    private float changePageBufferTimer;

    [Header("Bool")] 
    public bool canOpen = true;
    public bool canChangePage;
    public bool isOpened;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!canChangePage)
        {
            changePageBufferTimer += Time.deltaTime;
            if (changePageBufferTimer > changePageBuffer)
            {
                changePageBufferTimer = 0;
                canChangePage = true;
            }
        }
        
        if (isOpened && Controller.instance.moveInput.x > 0.5f && canChangePage && openedPage < maxPages)
        {
            ChangePageRight();
        }
        
        if (isOpened && Controller.instance.moveInput.x < -0.5f  && canChangePage && openedPage > 0)
        {
            ChangePageLeft();
        }
    }

    public void OpenCloseCarnet()
    {
        if (Controller.instance.isGrounded && !CinématiqueManager.instance.isCinématique && canOpen)
        {
            if (isOpened)
            {
                isOpened = false;
                carnet.DOFade(0, 0.5f);
                MapManager.instance.MapMenu.DOFade(0, 0.5f);
                Controller.instance.canJump = true;
                Controller.instance.canMove = true;   
                KeyUI.instance.HideMapKey();
                
                foreach (Image img in MapManager.instance.doneFilterList)
                {
                    img.DOFade(0, 0.5f);
                }
            }
            else
            {
                isOpened = true;
                MapManager.instance.MovePlayerIcon();
                carnet.DOFade(1, 0.5f);
                MapManager.instance.MapMenu.DOFade(1, 0.5f);
                Controller.instance.canJump = false;
                Controller.instance.canMove = false;  
                KeyUI.instance.ShowMapKey();

                if (KeyUI.instance.keyInRegion["Village"] == 0) // Met les zones en couleur si toutes les clés sont récupérées
                {
                    MapManager.instance.doneFilterList[0].DOFade(1, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Bosquet"] == 0)
                {
                    MapManager.instance.doneFilterList[1].DOFade(1, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Hameau"] == 0)
                {
                    MapManager.instance.doneFilterList[2].DOFade(1, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Plaine"] == 0)
                {
                    MapManager.instance.doneFilterList[3].DOFade(1, 1.2f);
                }
            }
        }
    }

    public void ChangePageRight()
    {
        canChangePage = false;
        pagesList[openedPage].SetActive(false);
        openedPage += 1;
        pagesList[openedPage].SetActive(true);
    }
    
    public void ChangePageLeft()
    {
        canChangePage = false;
        pagesList[openedPage].SetActive(false);
        openedPage -= 1;
        pagesList[openedPage].SetActive(true);
    }
}
