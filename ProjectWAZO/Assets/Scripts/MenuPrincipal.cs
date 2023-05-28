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
    public PlayableDirector director;
    public UnityEngine.EventSystems.EventSystem EventSystem;
    public Button boutonStart;
    public GameObject dropdonOptions;
    public CanvasGroup MenuMain;
    public CanvasGroup MenuOptions;
    public GameObject cameraMenu;
    public GameObject cameraCine;
    public Vector3 mainPos;
    public Vector3 optionsPos;
    public GameObject currentlySelected;
    public Image blackScreen;
    public GameObject mainMusic;

    private void Awake()
    {
        if (enableMainMenu)
        {
            mainMusic.SetActive(true);
            director.playOnAwake = false;
            EventSystem.SetSelectedGameObject(boutonStart.gameObject);
        }
        else
        { 
            mainMusic.SetActive(true);
            cameraMenu.SetActive(false);
            cameraCine.SetActive(true);
            director.Play();
            EventSystem.SetSelectedGameObject(null);
        }
    }
    
    private void Update()
    {
        if (currentlySelected != EventSystem.currentSelectedGameObject)
        {
            EventSystem.currentSelectedGameObject.gameObject.transform.DOScale(gameObject.transform.localScale * 1.2f, 0.2f);
            currentlySelected.transform.DOScale(currentlySelected.transform.localScale * 0.8f, 0.2f);
            currentlySelected = EventSystem.currentSelectedGameObject;
        }
    }

    public void StartGame()
    {
        
        EventSystem.SetSelectedGameObject(null);
        currentlySelected = EventSystem.currentSelectedGameObject;
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick2, 1f);
        MenuMain.interactable = false;
        MenuMain.blocksRaycasts = false;
        mainMusic.SetActive(false);
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
        EventSystem.SetSelectedGameObject(dropdonOptions.gameObject);
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick1, 1f);
        
        cameraMenu.transform.DOLocalMove(optionsPos,1.5f);
        MenuMain.interactable = false;
        MenuMain.blocksRaycasts = false;
        MenuOptions.interactable = true;
        MenuOptions.blocksRaycasts = true;
    }
    
    public void QuitOptions()
    {
        EventSystem.SetSelectedGameObject(boutonStart.gameObject);
        currentlySelected = EventSystem.currentSelectedGameObject;
        AudioList.Instance.PlayOneShot(AudioList.Instance.uiClick3, 1f);
        cameraMenu.transform.DOLocalMove(mainPos,1.5f);
        MenuMain.interactable = true;
        MenuMain.blocksRaycasts = true;
        MenuOptions.interactable = false;
        MenuOptions.blocksRaycasts = false;
    }
    
    public void Quitter()
    {
        Application.Quit();
    }
}
