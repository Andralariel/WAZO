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
    public Image Contour;
    public Dictionary<string, int> keyInRegion = new Dictionary<string, int>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        myRect = GetComponent<RectTransform>();
        
        keyInRegion.Add("Village",3);
        keyInRegion.Add("Bosquet",2);
        keyInRegion.Add("Hameau",2);
        keyInRegion.Add("Plaine",3);
        /*foreach (KeyValuePair<string,int> oui in keyInRegion)
        {
            Debug.Log(oui.Key + oui.Value);
        }*/
    }

    public void ShowKey() // Affiche l'UI
    {
        myRect.DOAnchorPos(showPosition, 0.5f);
        StartCoroutine(HideKey(2f));
    }

    public void RegisterKey(int ID) // Enregistre la clé comme récupérée et update l'UI en conséquence
    {
        MapManager.instance.listCroix[ID].gameObject.SetActive(true);
      

        if (keyObjectList[ID].choseRegion == KeyShard.Region.Bosquet)
        {
            keyInRegion["Bosquet"] -= 1;
        }
        else if (keyObjectList[ID].choseRegion == KeyShard.Region.Village)
        {
            keyInRegion["Village"] -= 1;
        }
        else if (keyObjectList[ID].choseRegion == KeyShard.Region.Plaine)
        {
            keyInRegion["Plaine"] -= 1;
        }
        else if (keyObjectList[ID].choseRegion == KeyShard.Region.Hameau)
        {
            keyInRegion["Hameau"] -= 1;
        }

        if (currentShard == TempleOpener.instance.AmountToOpen)
        {
            MapManager.instance.keyDoneIcon.gameObject.SetActive(true);
            Contour.DOColor(Color.yellow, 0.5f);
        }

    }
    
    public IEnumerator HideKey(float timeToHide) // Faire apparaitre un morceau de clé puis faire disparaitre l'UI
    {
        yield return new WaitForSeconds(timeToHide/2);
        shardImageList[currentShard - 1].DOFade(1, 0.5f);
        yield return new WaitForSeconds(timeToHide);
        myRect.DOAnchorPos(hidePosition, 0.5f).OnComplete((() =>   MapManager.instance.IconMapUpdate(1.5f)));
    }

    public void FadeInBlackScreen(float duration) // Fade out et Fade in un écran noir
    {
        blackScreen.DOFade(1, duration);
    }
    
    public void FadeOutBlackScreen(float duration)
    {
        blackScreen.DOFade(0, duration);
    }
    
    
    public void ShowMapKey() // Faire apparaite et disparaitre l'UI dans le menu de la carte
    {
        myRect.DOAnchorPos(showPosition, 0.5f);
    }
    
    public void HideMapKey()
    {
        myRect.DOAnchorPos(hidePosition, 0.5f);
    }
}
