using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using _3C;
using DG.Tweening;
using Sound;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utilitaire;

public class PauseMenu : MonoBehaviour
{
    public GameObject currentlySelected;
    public bool isScaling;
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
    public GameObject boutonRetour;
    public TMPro.TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public AudioMixer mixer;

    [Header("Triche")] 
    public bool isTriche;
    public CanvasGroup menuTriche;
    public GameObject boutonTpVillage;
    public GameObject TPPVillage;
    public GameObject TPPHamlet;
    public GameObject TPPGraveyard;
    public GameObject TPPTemple;
    public GameObject boutonTriche;
    public GameObject boutonRetourTriche;
    
  
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
                Controller.instance.ultraBlock = true;
                AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 0.4f);
                boutonReprendre.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
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
                Controller.instance.ultraBlock = false;
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
        
        if (isOption && !isTriche)
        {
            CloseOptions();
        }

        if (isTriche && !isOption)
        {
            CloseTriche();
            boutonReprendre.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
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
    }

    private void Update()
    {
        if (isPause && currentlySelected is not null)
        {
            if (currentlySelected != eventSystem.currentSelectedGameObject)
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
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            } 
        }
    }

    IEnumerator StopScaling()
    {
        yield return new WaitForSeconds(0.1f);
        isScaling = false;
    }

    public void QuitMenu()
    {
        if (isPause)
        {
            if (isOption && !isTriche) // quitte les options
            {
                AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
                CloseOptions();
            }
            
            if(!isOption && !isTriche)// quitte la pause
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

            if (isTriche && !isOption) // quitte la triche
            {
                CloseTriche();
            }
        }
    }

    public void AbleDesableTextWeight()
    {
        enableText = !enableText;
    }

    public void OpenOptions()
    {
        resolutionDropdown.transform.DOScale(Vector3.one, 0.2f);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 0.4f);
        isOption = true;
        eventSystem.SetSelectedGameObject(firstSelecOptions);
        currentlySelected = firstSelecOptions;
        boutonOptions.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
        firstSelecOptions.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        CG.interactable = false;
        CG.blocksRaycasts = false;
        CG.DOFade(0, 0.5f);
        
        canvasOptions.interactable = true;
        canvasOptions.blocksRaycasts = true;
        canvasOptions.DOFade(1, 0.5f);
    }

    public void CloseOptions()
    {
        boutonRetour.transform.DOScale(Vector3.one, 0.2f);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
        isOption = false;
        eventSystem.SetSelectedGameObject(boutonReprendre);
        currentlySelected = boutonReprendre;
        boutonReprendre.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
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

        currentlySelected = firstSelecOptions;
        firstSelecOptions.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
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
        currentlySelected = resolutionDropdown.gameObject;
        resolutionDropdown.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        isScaling = false;
    }
    
    //---------------------------Fonction pour les Triches-----------------------------------

    public void OpenTriche()
    {
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 0.4f);
        boutonTriche.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
        boutonRetourTriche.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
        boutonTpVillage.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        isTriche = true;
        eventSystem.SetSelectedGameObject(boutonTpVillage);
        currentlySelected = boutonTpVillage;
        menuTriche.DOFade(1, 0.5f);
        CG.DOFade(0, 0.5f);
        CG.interactable = false;
        CG.blocksRaycasts = false;
        menuTriche.interactable = true;
        menuTriche.blocksRaycasts = true;
    }
    
    public void CloseTriche()
    {
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 0.4f);
        boutonRetourTriche.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.2f);
        boutonReprendre.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.2f);
        isTriche = false;
        eventSystem.SetSelectedGameObject(boutonReprendre);
        currentlySelected = boutonReprendre;
        menuTriche.DOFade(0, 0.5f);
        CG.DOFade(1, 0.5f);
        CG.interactable = true;
        CG.blocksRaycasts = true;
        menuTriche.interactable = false;
        menuTriche.blocksRaycasts = false;
    }

    public void TPVillage()
    {
        Controller.instance.transform.position = TPPVillage.transform.position;
        Controller.instance.rb.velocity = Vector3.zero;
    }
    
    public void TPHamlet()
    {
        Controller.instance.transform.position = TPPHamlet.transform.position;
        Controller.instance.rb.velocity = Vector3.zero;
    }
    
    public void TPGraveyard()
    {
        Controller.instance.transform.position = TPPGraveyard.transform.position;
        Controller.instance.rb.velocity = Vector3.zero;
    }
    
    public void TPTemple()
    {
        Controller.instance.transform.position = TPPTemple.transform.position;
        Controller.instance.rb.velocity = Vector3.zero;
    }
    
    public void OnOffTemple()
    {
        TempleOpener.instance.canOpen = !TempleOpener.instance.canOpen;
    }

    public void OpenEscalier1()
    {
        if (!TempleManager.instance.isCine)
        {
            CloseTriche();
            CloseOptions();
            QuitMenu();
            TempleManager.instance.indexCalling = 1;
            TempleManager.instance.Activate();
        }
    }
    
    public void OpenEscalier2()
    {
        if (!TempleManager.instance.isCine)
        {
            CloseTriche();
            CloseOptions();
            QuitMenu();
            TempleManager.instance.indexCalling = 2;
            TempleManager.instance.Activate();
        }
    }
}
