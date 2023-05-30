using System;
using System.Collections;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class MenuPrincipal : MonoBehaviour
{
    public bool enableMainMenu;
    public bool isScaling;
    public PlayableDirector director;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    
    public Button boutonStart;
    public GameObject dropDownQuality;
    public GameObject dropDownResolution;
    public GameObject boutonOptions;
    public GameObject boutonRetour;
    public GameObject boutonReprendre;
    
    public CanvasGroup MenuMain;
    public CanvasGroup MenuOptions;
    public GameObject cameraMenu;
    public GameObject cameraCine;
    public Vector3 mainPos;
    public Vector3 optionsPos;
    public GameObject currentlySelected;
    public Image blackScreen;

    private void Awake()
    {
        if (enableMainMenu)
        {
            director.playOnAwake = false;
            eventSystem.SetSelectedGameObject(boutonStart.gameObject);
        }
        else
        { 
            cameraMenu.SetActive(false);
            cameraCine.SetActive(true);
            director.Play();
            eventSystem.SetSelectedGameObject(null);
        }
    }

    private void Start()
    {
        AudioList.Instance.StartMusic(AudioList.Music.main, true);
    }
    private void Update()
    {
        if (currentlySelected is not null && currentlySelected != eventSystem.currentSelectedGameObject)
        {
            if (!isScaling)
            {
                isScaling = true;
                try
                {
                    //eventSystem.currentSelectedGameObject.gameObject.transform.DOScale(gameObject.transform.localScale * 1.2f, 0.2f)
                     //   .OnComplete((() => StartCoroutine(StopScaling())));
                    currentlySelected.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f).OnComplete((() => isScaling = false));
                    currentlySelected = eventSystem.currentSelectedGameObject;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        } 
    }
    IEnumerator StopScaling()
    {
        yield return new WaitForSeconds(0.1f);
        isScaling = false;
    }

    public void StartGame()
    {
        
        eventSystem.SetSelectedGameObject(null);
        currentlySelected = eventSystem.currentSelectedGameObject;
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 0.4f);
        MenuMain.interactable = false;
        MenuMain.blocksRaycasts = false;
        AudioList.Instance.StopMusic(); // Arrêt de la musique du menu principal
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        blackScreen.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1.5f);
        blackScreen.DOFade(0, 1f);
        cameraMenu.SetActive(false);
        cameraCine.SetActive(true);
        director.Play();
        yield return new WaitForSeconds((float)director.duration-0.5f);
        blackScreen.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        AudioList.Instance.PlayOneShot(AudioList.Instance.crashGround, AudioList.Instance.crashGroundVolume);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Dev_map");
    }
    
    public void OpenOptions()
    {
        currentlySelected = dropDownQuality.gameObject;
        eventSystem.SetSelectedGameObject(dropDownQuality.gameObject);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick1, 0.4f);
        MenuMain.interactable = false;
        MenuMain.blocksRaycasts = false;
        MenuOptions.interactable = true;
        MenuOptions.blocksRaycasts = true;
        MenuMain.DOFade(0, 0.5f);
        MenuOptions.DOFade(1, 0.5f);
        boutonOptions.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f);
        dropDownResolution.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f);
        dropDownQuality.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f);
    }
    
    public void QuitOptions()
    {
        eventSystem.SetSelectedGameObject(boutonStart.gameObject);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
        cameraMenu.transform.DOLocalMove(mainPos,1.5f);
        MenuMain.interactable = true;
        MenuMain.blocksRaycasts = true;
        MenuOptions.interactable = false;
        MenuOptions.blocksRaycasts = false;
        MenuMain.DOFade(1, 0.5f);
        MenuOptions.DOFade(0, 0.5f);
        boutonRetour.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f);
        currentlySelected = boutonReprendre;
        boutonReprendre.transform.DOScale(boutonReprendre.transform.localScale *1.2f, 0.2f);
    }
    
    public void Quitter()
    {
        Application.Quit();
    }
}
