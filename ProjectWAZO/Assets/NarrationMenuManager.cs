using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NarrationMenuManager : MonoBehaviour
{
    public static NarrationMenuManager instance;
    private CanvasGroup myCG;
    public bool isOpen;
    public Image displayedFresque;
    public List<Sprite> fresqueList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        myCG = GetComponent<CanvasGroup>();
    }

    public void OpenMenu()
    {
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        isOpen = true;
        myCG.DOFade(1, 0.5f);
    }
    
    public void CloseMenu()
    {
        if (isOpen)
        {
            Controller.instance.canMove = true;
            Controller.instance.canJump = true;
            isOpen = false;
            myCG.DOFade(0, 0.5f); 
        }
    }
    
    public void ChangeFresque(int ID)
    {
        displayedFresque.sprite = fresqueList[ID];
    }
}
