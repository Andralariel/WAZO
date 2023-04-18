using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public static KeyUI instance;
    private RectTransform myRect;
    public Vector2 showPosition;
    public Vector2 hidePosition;
    public List<Image> shardImageList;
    public List<KeyShard> keyObjectList;
    public int currentShard;
    public Image blackScreen;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        myRect = GetComponent<RectTransform>();
    }

    public void ShowKey()
    {
        //transform.DOMove(transform.position-new Vector3(showHideDistance,0,0), 0.5f);
        myRect.DOAnchorPos(showPosition, 0.5f);
        StartCoroutine(HideKey(2f));
    }

    public void RegisterKey(int ID)
    {
        MapManager.instance.listCroix[ID].gameObject.SetActive(true);
    }
    
    public IEnumerator HideKey(float timeToHide)
    {
        yield return new WaitForSeconds(timeToHide/2);
        shardImageList[currentShard - 1].DOFade(1, 0.5f);
        yield return new WaitForSeconds(timeToHide);
        //transform.DOMove(transform.position+new Vector3(showHideDistance,0,0), 0.5f);
        myRect.DOAnchorPos(hidePosition, 0.5f);
    }

    public void FadeInBlackScreen(float duration)
    {
        blackScreen.DOFade(1, duration);
    }
    
    public void FadeOutBlackScreen(float duration)
    {
        blackScreen.DOFade(0, duration);
    }
    
    
    public void ShowMapKey()
    {
       // transform.DOMove(transform.position-new Vector3(showHideDistance,0,0), 0.5f);
       myRect.DOAnchorPos(showPosition, 0.5f);
    }
    
    public void HideMapKey()
    {
       // transform.DOMove(transform.position+new Vector3(showHideDistance,0,0), 0.5f);
       myRect.DOAnchorPos(hidePosition, 0.5f);
    }
}
