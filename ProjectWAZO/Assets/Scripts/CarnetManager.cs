using System.Collections.Generic;
using _3C;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarnetManager : MonoBehaviour
{
    public static CarnetManager instance;
    public List<GameObject> pagesList;
    public CanvasGroup carnet;
    public Animator anim;
    public Image imageTuTo;
    public GameObject map;
    

    [Header("Bool")] public bool firstTime;
    public bool canOpen = true;
    public bool isOpened;
    public bool isOptions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OpenCloseCarnet()
    {
        if (Controller.instance.isGrounded && !CinématiqueManager.instance.isCinématique && canOpen && !PauseMenu.instance.isOption)
        {
            if (isOpened)
            {
                anim.SetBool("isOpen",false);
                isOpened = false;
                carnet.DOFade(0, 0.5f);
                MapManager.instance.MapMenu.DOFade(0, 0.5f);
                Controller.instance.canJump = true;
                Controller.instance.ultraBlock = false;
                Controller.instance.canMove = true;   
                KeyUI.instance.HideMapKey();
                
                foreach (Image img in MapManager.instance.doneFilterList)
                {
                    img.DOFade(0, 0.5f);
                }
            }
            else
            {
                anim.SetBool("isOpen",true);
                canOpen = false;
                isOpened = true;
                MapManager.instance.MovePlayerIcon();
                MapManager.instance.MapMenu.DOFade(1, 0.5f)
                    .OnComplete((() => carnet.DOFade(1, 0.5f)));
                StartCoroutine(KeyUI.instance.ShowMapKeyWithDelay(0.8f));
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
        if (isOpened)
        {
            isOpened = false;
            carnet.DOFade(0, 0.5f);
            MapManager.instance.MapMenu.DOFade(0, 0.5f);
            Controller.instance.canJump = true;
            Controller.instance.canMove = true;   
            KeyUI.instance.HideMapKey();
                
            foreach (Image img in MapManager.instance.doneFilterList)
            {
                img.DOFade(0, 0.5f);
            }
        }
    }
}
