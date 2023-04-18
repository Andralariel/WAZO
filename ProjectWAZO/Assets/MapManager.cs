using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    
    public static MapManager instance;
    public bool isMapOpened;
    public CanvasGroup MapMenu;
    public List<Image> listCroix;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    

    public void OpenCloseMap()
    {
        if (Controller.instance.isGrounded)
        {
            if (isMapOpened)
            {
                isMapOpened = false;
                MapMenu.DOFade(0, 0.5f);
                Controller.instance.canJump = true;
                Controller.instance.canMove = true;   
                KeyUI.instance.HideMapKey();
            }
            else
            {
                isMapOpened = true;
                MapMenu.DOFade(1, 0.5f);
                Controller.instance.canJump = false;
                Controller.instance.canMove = false;  
                KeyUI.instance.ShowMapKey();
            }
        }
    }
}
