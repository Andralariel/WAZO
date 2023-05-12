using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public bool isPause;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public GameObject boutonReprendre;
    public GameObject boutonRetour;
    private CanvasGroup CG;
    public CanvasGroup canvasOptions;
    public static PauseMenu instance;
    
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        CG = GetComponent<CanvasGroup>();
    }

    public void PauseUnPause()
    {
        if (!isPause)
        {
            isPause = true;
            CG.interactable = true;
            CG.blocksRaycasts = true;
            CG.DOFade(1, 0.5f);
            eventSystem.SetSelectedGameObject(boutonReprendre);
            CinématiqueManager.instance.isCinématique = false;
            Controller.instance.canMove = false;
            Controller.instance.canJump = false;
        }
        else
        {
            isPause = false;
            CG.interactable = false;
            CG.blocksRaycasts = false;
            CG.DOFade(0, 0.5f);
            eventSystem.SetSelectedGameObject(null);
            CinématiqueManager.instance.isCinématique = true;
            Controller.instance.canMove = true;
            Controller.instance.canJump = true;
        }
           
    }

    public void OpenOptions()
    {
        eventSystem.SetSelectedGameObject(boutonRetour);
        CG.interactable = false;
        CG.blocksRaycasts = false;
        CG.DOFade(0, 0.5f);
        
        canvasOptions.interactable = true;
        canvasOptions.blocksRaycasts = true;
        canvasOptions.DOFade(1, 0.5f);
    }

    public void CloseOptions()
    {
        eventSystem.SetSelectedGameObject(boutonReprendre);
        CG.interactable = true;
        CG.blocksRaycasts = true;
        CG.DOFade(1, 0.5f); 
        
        canvasOptions.interactable = false;
        canvasOptions.blocksRaycasts = false;
        canvasOptions.DOFade(0, 0.5f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
