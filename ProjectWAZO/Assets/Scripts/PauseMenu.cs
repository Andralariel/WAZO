using System;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using Sound;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject currentlySelected;
    public bool isScaling;
    public bool bufferScaleBouton;
    public bool isPause;
    public bool canPause;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public GameObject boutonReprendre;
    public GameObject boutonOptions;
    public GameObject firstSelecOptions;
    private CanvasGroup CG;
    public CanvasGroup canvasOptions;
    public static PauseMenu instance;
    public bool enableText;

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
        if (!isOption && canPause && !CarnetManager.instance.isOpened)
        {
            if (!isPause)
            {
                isScaling = true;
                if (isScaling && bufferScaleBouton)
                {
                    bufferScaleBouton = false;
                    isScaling = true;
                    boutonReprendre.transform.DOScale( boutonReprendre.transform.localScale * 1.2f, 0.2f).OnComplete((() => isScaling = false));
                    boutonReprendre.transform.DOMove(boutonReprendre.transform.position, 0.2f)
                        .OnComplete((() => bufferScaleBouton = true));
                }
                
                AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 0.4f);
                boutonReprendre.transform.DOScale(boutonReprendre.transform.localScale * 1.2f, 0.2f);
                isPause = true;
                CG.interactable = true;
                CG.blocksRaycasts = true;
                CG.DOFade(1, 0.5f);
                eventSystem.SetSelectedGameObject(boutonReprendre);
                currentlySelected = boutonReprendre;
                CinématiqueManager.instance.isCinématique = true;
                Controller.instance.ultraBlock = true;
                Controller.instance.canMove = false;
                Controller.instance.canJump = false;
            }
            else
            {
                bufferScaleBouton = true;
                isScaling = false;
                boutonReprendre.transform.localScale = Vector3.one;
                AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
                isPause = false;
                CG.interactable = false;
                CG.blocksRaycasts = false;
                CG.DOFade(0, 0.5f);
                eventSystem.SetSelectedGameObject(null);
                currentlySelected = null;
                CinématiqueManager.instance.isCinématique = false;
                Controller.instance.ultraBlock = false;
                Controller.instance.canMove = true;
                Controller.instance.canJump = true;
            }
        }
        else if (isOption)
        {
            CloseOptions();
        }
    }

    private void Update()
    {
        if (isPause && currentlySelected is not null)
        {
            if (currentlySelected != eventSystem.currentSelectedGameObject)
            {
                if (!isScaling && bufferScaleBouton)
                {
                    isScaling = true;
                    eventSystem.currentSelectedGameObject.gameObject.transform.DOScale(gameObject.transform.localScale * 1.2f, 0.2f);
                    currentlySelected.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f).OnComplete((() => isScaling = false));
                    currentlySelected = eventSystem.currentSelectedGameObject;
                }
            } 
        }
    }

    public void QuitMenu()
    {
        if (isPause)
        {
            if (isOption) // quitte les options
            {
                AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
                CloseOptions();
            }
            else // quitte la pause
            {
                if (isScaling)
                {
                    currentlySelected.transform.localScale = Vector3.one;
                    eventSystem.currentSelectedGameObject.transform.localScale = Vector3.one;
                }
                AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
                isPause = false;
                CG.interactable = false;
                CG.blocksRaycasts = false;
                CG.DOFade(0, 0.5f);
                eventSystem.SetSelectedGameObject(null);
                currentlySelected = null;
                CinématiqueManager.instance.isCinématique = false;
                Controller.instance.ultraBlock = false;
                Controller.instance.canMove = true;
                Controller.instance.canJump = true;
            }
        }
    }

    public void AbleDesableTextWeight()
    {
        enableText = !enableText;
    }

    public void OpenOptions()
    {
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 0.4f);
        isOption = true;
        eventSystem.SetSelectedGameObject(firstSelecOptions);
        currentlySelected = firstSelecOptions;
        boutonOptions.transform.DOScale(boutonOptions.transform.localScale * 0.8f, 0.2f);
        firstSelecOptions.transform.DOScale(firstSelecOptions.transform.localScale * 1.2f, 0.2f);
        CG.interactable = false;
        CG.blocksRaycasts = false;
        CG.DOFade(0, 0.5f);
        
        canvasOptions.interactable = true;
        canvasOptions.blocksRaycasts = true;
        canvasOptions.DOFade(1, 0.5f);
    }

    public void CloseOptions()
    {
        currentlySelected.transform.DOScale(Vector3.one, 0.2f);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
        isOption = false;
        eventSystem.SetSelectedGameObject(boutonReprendre);
        currentlySelected = boutonReprendre;
        boutonReprendre.transform.DOScale(boutonReprendre.transform.localScale * 1.2f, 0.2f);
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
