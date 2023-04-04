using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Vector3 showPosition;
    public Vector3 hidePosition;
    public List<Image> shardList;
    public int currentShard;
    public static KeyUI instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Show()
    {
        transform.DOMove(showPosition, 0.5f);
        StartCoroutine(Hide(2f));
    }
    
    public IEnumerator Hide(float timeToHide)
    {
        yield return new WaitForSeconds(timeToHide/2);
        shardList[currentShard - 1].DOFade(1, 0.5f);
        yield return new WaitForSeconds(timeToHide);
        transform.DOMove(hidePosition, 0.5f);
    }
    
}
