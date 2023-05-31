using System;
using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
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
    public TMPro.TMP_Dropdown dropDownResolution;
    public GameObject boutonOptions;
    public GameObject boutonRetour;
    public GameObject boutonReprendre;
    
    public CanvasGroup MenuMain;
    public CanvasGroup MenuOptions;
    public Camera cameraMenu;
    public GameObject cameraCine;
    public GameObject currentlySelected;
    public Image blackScreen;
    public Image bandeNoire1;
    public Image bandeNoire2;
    public AudioMixer mixer;

    [Header("Ghetto")] 
    public bool zoomCam;
    public float zoomSpeed;
    public float bloomSpeed;
    public Volume globalVolume;
    public GameObject boutonQuit;
    public GameObject fondStart;
    public GameObject fondOptions;
    public GameObject fondQuitter;
    
    
    private Resolution[] resolutions;

    private void Awake()
    {
        if (enableMainMenu)
        {
            director.playOnAwake = false;
            eventSystem.SetSelectedGameObject(boutonStart.gameObject);
        }
        else
        { 
            cameraMenu.gameObject.SetActive(false);
            cameraCine.SetActive(true);
            director.Play();
            eventSystem.SetSelectedGameObject(null);
        }
    }

    private void Start()
    {
        AudioList.Instance.StartMusic(AudioList.Music.main, true);
        boutonStart.gameObject.transform.DOScale(gameObject.transform.localScale * 1.2f, 0.2f);
        
        // Pour les résolutions
        resolutions = Screen.resolutions;
        dropDownResolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        dropDownResolution.AddOptions(options);
        dropDownResolution.value = currentResolutionIndex;
        dropDownResolution.RefreshShownValue();
        
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
                    eventSystem.currentSelectedGameObject.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f)
                        .OnComplete((() => StartCoroutine(StopScaling())));
                    currentlySelected.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f).OnComplete((() => isScaling = false));
                    currentlySelected = eventSystem.currentSelectedGameObject;

                    if (currentlySelected == boutonReprendre)
                    {
                        fondStart.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
                    }
                    else
                    {
                        fondStart.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f);
                    }
                    
                    if (currentlySelected == boutonOptions)
                    {
                        fondOptions.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
                    }
                    else
                    {
                        fondOptions.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f);
                    }
                    
                    if (currentlySelected == boutonQuit)
                    {
                        fondQuitter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
                    }
                    else
                    {
                        fondQuitter.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        /*if (zoomCam)
        {
            globalVolume.weight += bloomSpeed * Time.deltaTime;
            if (cameraMenu.orthographicSize >= 0.1f)
            {
                cameraMenu.orthographicSize -= zoomSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (globalVolume.weight >= 0)
            {
                globalVolume.weight -= bloomSpeed * Time.deltaTime;   
            }
           
        }*/
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
        /*zoomCam = true;
        yield return new WaitForSeconds(2.5f);
        bandeNoire1.gameObject.SetActive(true);
        bandeNoire2.gameObject.SetActive(true);
        cameraMenu.gameObject.SetActive(false);
        cameraCine.SetActive(true);
        globalVolume.weight = 1;
        yield return new WaitForSeconds(1f);
        zoomCam = false;
        yield return new WaitForSeconds(0.5f);*/
        
        blackScreen.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bandeNoire1.gameObject.SetActive(true);
        bandeNoire2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackScreen.DOFade(0, 1f);
        cameraMenu.gameObject.SetActive(false);
        cameraCine.SetActive(true);
        director.Play();
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
        boutonOptions.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
        dropDownQuality.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        dropDownResolution.gameObject.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
    }
    
    public void QuitOptions()
    {
        fondStart.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        eventSystem.SetSelectedGameObject(boutonStart.gameObject);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
        MenuMain.interactable = true;
        MenuMain.blocksRaycasts = true;
        MenuOptions.interactable = false;
        MenuOptions.blocksRaycasts = false;
        MenuMain.DOFade(1, 0.5f);
        MenuOptions.DOFade(0, 0.5f);
        boutonRetour.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
        currentlySelected = boutonReprendre;
        boutonReprendre.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        dropDownQuality.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        dropDownResolution.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
    }
    
    public void ChangeQuality(int index)
    {
        if (index == 0)
        {
            QualitySettings.SetQualityLevel(2,true);   
        }
        else if (index == 1)
        {
            QualitySettings.SetQualityLevel(1,true);   
        }
        else if (index == 2)
        {
            QualitySettings.SetQualityLevel(0,true);   
        }

        currentlySelected = dropDownQuality;
        dropDownQuality.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        isScaling = false;
    }
    
    public void SetMasterLevel(float sliderValue)
    {
        // mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) *40);
        mixer.SetFloat("MasterVol", (-80 + sliderValue*100));
    }
    
    public void SetMusicLevel(float sliderValue)
    {
        //mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) *40);
        mixer.SetFloat("MusicVol", (-80 + sliderValue*100));
    }
    
    public void SetSFXLevel(float sliderValue)
    {
        //mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) *40);
        mixer.SetFloat("SFXVol", (-80 + sliderValue*100));
    }
    
    public void ChangeFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("changement fullscrren");
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        currentlySelected = dropDownResolution.gameObject;
        dropDownResolution.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        isScaling = false;
    }
    
    public void Quitter()
    {
        Application.Quit();
    }
}
