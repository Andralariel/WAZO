using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    public UnityEngine.EventSystems.EventSystem EventSystem;
    public Button boutonStart;
    public Button boutonRetour;
    public bool EnableMainMenu;
    public CanvasGroup MenuMain;
    public CanvasGroup MenuOptions;

    private void Start()
    {
        if (EnableMainMenu)
        {
            CinématiqueManager.instance.isCinématique = true;
            EventSystem.SetSelectedGameObject(boutonStart.gameObject);
            CameraController.instance.canMove = false;
            CameraController.instance.StartToPlayer = false;
            CameraController.instance.transform.localPosition = new Vector3(-820, 0, 0);
        }
    }

    public void StartGame()
    {
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position - new Vector3(60,0,0), 2f)
            .OnComplete((() =>   CinématiqueManager.instance.StartCinématique(0)));
        boutonStart.interactable = false;
    }
    
    public void OpenOptions()
    {
        EventSystem.SetSelectedGameObject(boutonRetour.gameObject);
        CameraController.instance.transform.DORotate(new Vector3(270f,-90f,0),1.5f);
        MenuMain.DOFade(0, 0.3f);
        MenuMain.interactable = false;
        MenuMain.blocksRaycasts = false;
        MenuOptions.DOFade(1, 1.7f);
        MenuOptions.interactable = true;
        MenuOptions.blocksRaycasts = true;
    }
    
    public void QuitOptions()
    {
        EventSystem.SetSelectedGameObject(boutonStart.gameObject);
        CameraController.instance.transform.DORotate(new Vector3(0,-90f,0),1.5f);
        MenuMain.DOFade(1, 0.3f);
        MenuMain.interactable = true;
        MenuMain.blocksRaycasts = true;
        MenuOptions.DOFade(0, 1.7f);
        MenuOptions.interactable = false;
        MenuOptions.blocksRaycasts = false;
    }
    
    public void Quitter()
    {
        Application.Quit();
    }
}
