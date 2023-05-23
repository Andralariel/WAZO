using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
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

    private void Awake()
    {
        if (enableMainMenu)
        {
            director.playOnAwake = false;
            EventSystem.SetSelectedGameObject(boutonStart.gameObject);
        }
        else
        {
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
        cameraMenu.SetActive(false);
        cameraCine.SetActive(true);
        EventSystem.SetSelectedGameObject(null);
        currentlySelected = EventSystem.currentSelectedGameObject;
        director.Play();
        MenuMain.interactable = false;
        MenuMain.blocksRaycasts = false;
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds((float)director.duration);
        SceneManager.LoadScene("Dev_map");
    }
    
    public void OpenOptions()
    {
        EventSystem.SetSelectedGameObject(dropdonOptions.gameObject);
        
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
