using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.UI;

public class CarnetManager : MonoBehaviour
{
    public static CarnetManager instance;
    public CanvasGroup carnet;
    public Animator anim;
    public Image imageTuTo;
    public GameObject map;
    public Image blackScreen;
    

    [Header("Bool")] public bool firstTime;
    public bool canOpen = true;
    public bool isOpened;
    public bool isOptions;
    public bool enableStartBlackScreen;

    private void Awake()
    {
        if (instance != default && instance!=this)
        {
            DestroyImmediate(this);
        }
        instance = this;

        if (enableStartBlackScreen)
        {
            blackScreen.color = Color.black;
            blackScreen.DOFade(0, 3f);
        }
    }

    public void OpenCloseCarnet()
    {
        if (Controller.instance.isGrounded && !CinématiqueManager.instance.isCinématique && canOpen && !PauseMenu.instance.isOption)
        {
            if (isOpened)
            {
                AudioList.Instance.PlayOneShot(AudioList.Instance.closeCarnet, AudioList.Instance.closeCarnetVolume);
                anim.SetBool("isOpen",false);
                isOpened = false;
                carnet.DOFade(0, 0.5f);
                MapManager.instance.MapMenu.DOFade(0, 0.5f);
                Controller.instance.canJump = true;
                Controller.instance.ultraBlock = false;
                Controller.instance.canMove = true;   
                KeyUI.instance.HideMapKey();
                KeyUI.instance.HideMapBonusKey();
                
                foreach (Image img in MapManager.instance.doneFilterList)
                {
                    img.DOFade(0, 0.5f);
                }
            }
            else
            {
                for (int i = 0; i < MapManager.instance.isFresqueUnlocked.Count; i++)
                {
                    if (MapManager.instance.isFresqueUnlocked[i])
                    {
                        StartCoroutine(MapManager.instance.UnlockFresque(i));
                    }
                }   
                AudioList.Instance.PlayOneShot(AudioList.Instance.openCarnet, AudioList.Instance.openCarnetVolume);
                anim.SetBool("isOpen",true);
                canOpen = false;
                isOpened = true;
                MapManager.instance.MovePlayerIcon();
                MapManager.instance.MapMenu.DOFade(1, 0.5f)
                    .OnComplete((() => carnet.DOFade(1, 0.5f)));
                StartCoroutine(KeyUI.instance.ShowMapKeyWithDelay(0.8f));
                StartCoroutine(KeyUI.instance.ShowMapBonusKeyWithDelay(0.8f));
                Controller.instance.anim.SetBool("isWalking",false);
                Controller.instance.anim.SetBool("isIdle",true);
                Controller.instance.canJump = false;
                Controller.instance.ultraBlock = true;
                Controller.instance.canMove = false;  
                
                if (KeyUI.instance.keyInRegion["Village"] <= 0) // Met les zones en couleur si toutes les clés sont récupérées
                {
                    MapManager.instance.doneFilterList[0].DOFade(0.9f, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Bosquet"] <= 0)
                {
                    MapManager.instance.doneFilterList[1].DOFade(0.9f, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Hameau"] <= 0)
                {
                    MapManager.instance.doneFilterList[2].DOFade(0.9f, 1.2f);
                }
                
                if (KeyUI.instance.keyInRegion["Plaine"] <= 0)
                {
                    MapManager.instance.doneFilterList[3].DOFade(0.9f, 1.2f);
                }
                if (KeyUI.instance.keyInRegion["Cimetière"] <= 0)
                {
                    MapManager.instance.doneFilterList[4].DOFade(0.9f, 1.2f);
                }
            }
        }
    }
    
    public void QuitMenu()
    {
        if (isOpened && canOpen)
        {
            AudioList.Instance.PlayOneShot(AudioList.Instance.closeCarnet, AudioList.Instance.closeCarnetVolume);
            anim.SetBool("isOpen",false);
            isOpened = false;
            MapManager.instance.canRotate = true;
            carnet.DOFade(0, 0.5f);
            MapManager.instance.MapMenu.DOFade(0, 0.5f);
            Controller.instance.canJump = true;
            Controller.instance.ultraBlock = false;
            Controller.instance.canMove = true;   
            KeyUI.instance.HideMapKey();
            KeyUI.instance.HideMapBonusKey();
                
            foreach (Image img in MapManager.instance.doneFilterList)
            {
                img.DOFade(0, 0.5f);
            }
        }
    }
}
