    using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public bool isPause;
    public bool canPause;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public GameObject boutonReprendre;
    public GameObject boutonRetour;
    private CanvasGroup CG;
    public CanvasGroup canvasOptions;
    public static PauseMenu instance;

    [Header("Options")] 
    public bool isOption;
    public TMPro.TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public AudioMixer mixer;
    
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        CG = GetComponent<CanvasGroup>();
        
        // Pour les résolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
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
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void PauseUnPause()
    {
        if (!isOption && canPause)
        {
            if (!isPause)
            {
                isPause = true;
                CG.interactable = true;
                CG.blocksRaycasts = true;
                CG.DOFade(1, 0.5f);
                eventSystem.SetSelectedGameObject(boutonReprendre);
                CinématiqueManager.instance.isCinématique = true;
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
                CinématiqueManager.instance.isCinématique = false;
                Controller.instance.canMove = true;
                Controller.instance.canJump = true;
            }
        }
        else
        {
            CloseOptions();
        }
    }

    public void OpenOptions()
    {
        isOption = true;
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
        isOption = false;
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
    
    
    
    //---------------------------Fonction pour les Options-----------------------------------

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
    }
}
