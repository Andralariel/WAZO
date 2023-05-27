using System.Collections.Generic;
using _3C;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NarrationMenuManager : MonoBehaviour
{
    public static NarrationMenuManager instance;
    private CanvasGroup myCG;
    public bool isOpen;
    public Image displayedFresque;
    public List<Sprite> fresqueList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        myCG = GetComponent<CanvasGroup>();
    }

    public void OpenMenu()
    {
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        isOpen = true;
        myCG.DOFade(1, 0.5f);
    }
    
    public void CloseMenu()
    {
        if (isOpen)
        {
            CarnetManager.instance.canOpen = true;
            Controller.instance.isGoing = false;
            Controller.instance._moveDir = Vector3.zero;
            Controller.instance.canMove = true;
            Controller.instance.canJump = true;
            isOpen = false;
            myCG.DOFade(0, 0.5f);
            CinématiqueManager.instance.isCinématique = false;
        }
    }
    
    public void ChangeFresque(int ID)
    {
        displayedFresque.sprite = fresqueList[ID];
    }
}
