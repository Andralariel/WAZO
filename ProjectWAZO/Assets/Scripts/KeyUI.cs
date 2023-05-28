using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilitaire;
using WeightSystem.Activator;

public class KeyUI : MonoBehaviour
{
    public static KeyUI instance;
    private RectTransform myRect;
    public Vector2 showPosition;
    public Vector2 hidePosition;
    public List<Image> shardImageList;
    public List<KeyShard> keyObjectList;
    public Image contour;
    public int currentShard;
    public int currentBonusShard;
    public CanvasGroup bonusKeyAlpha;
    public Image blackScreen;
    public TextMeshProUGUI compteur;
    public Dictionary<string, int> keyInRegion = new Dictionary<string, int>();

    [Header("Additional Shards")] 
    public Vector2 showBonusPosition;
    public Vector2 hideBonusPosition;
    public RectTransform addShard;
    public TextMeshProUGUI compteurBonus;
    public List<Image> addShardImageList;
    public Image lockedFilter;
    public Image lockImage;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        myRect = GetComponent<RectTransform>();
        
        keyInRegion.Add("Village",2);
        keyInRegion.Add("Bosquet",3);
        keyInRegion.Add("Hameau",2);
        keyInRegion.Add("Plaine",2);
        keyInRegion.Add("Cimetière",1);
    }

    public void RegisterKey(int ID) // Enregistre la clé comme récupérée et update l'UI en conséquence
    {
        MapManager.instance.listCroix[ID].gameObject.SetActive(true);
        MapManager.instance.pontIntero[ID].gameObject.SetActive(false);

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
        else if (keyObjectList[ID].choseRegion == KeyShard.Region.Cimetière)
        {
            keyInRegion["Cimetière"] -= 1;
        }
        
        if (currentShard == TempleOpener.instance.AmountToOpen)
        {
            contour.DOColor(Color.yellow, 4f);
        }

    }
    
    public void ShowKey() // Affiche l'UI
    {
        myRect.DOAnchorPos(showPosition, 0.5f);
        StartCoroutine(HideKey(2f));
    }
    public IEnumerator HideKey(float timeToHide) // Faire apparaitre un morceau de clé puis faire disparaitre l'UI
    {
        yield return new WaitForSeconds(timeToHide/2);
        if (currentShard <= 5)
        {
            shardImageList[currentShard].DOFade(0, 0.5f).OnComplete((() => currentShard += 1));;
            yield return new WaitForSeconds(0.5f);
            compteur.text = currentShard + " / 6"; 
        }
        yield return new WaitForSeconds(timeToHide-0.5f);

        if (currentShard != 6)
        {
            myRect.DOAnchorPos(hidePosition, 0.5f).OnComplete((() =>   MapManager.instance.IconMapUpdate(3)));
        }
        else
        {
            myRect.DOAnchorPos(hidePosition, 0.5f);
        }
      
    }
    
    public void ShowAdditionalKey() // Affiche l'UI
    {
        addShard.DOAnchorPos(showBonusPosition, 0.5f);
        StartCoroutine(HideAdditionalKey(2f));
    }
    public IEnumerator HideAdditionalKey(float timeToHide) // Faire apparaitre un morceau de clé puis faire disparaitre l'UI
    {
        yield return new WaitForSeconds(timeToHide/2);
        if (currentShard == 5)
        {
            bonusKeyAlpha.DOFade(1,0.5f);
        }
        
        if (currentShard >= 6)
        {
            addShardImageList[currentBonusShard].DOColor(Color.white, 0.5f).OnComplete((() => currentBonusShard++));
            yield return new WaitForSeconds(0.5f);
            compteurBonus.text = currentBonusShard + " / 4"; 
        }
        yield return new WaitForSeconds(timeToHide-0.5f);
        addShard.DOAnchorPos(hideBonusPosition, 0.5f).OnComplete((() =>   MapManager.instance.IconMapUpdate(3)));
    }

    public void UnlockAdditionalKey() // Affiche l'UI
    {
        
        StartCoroutine(HideUnlockAdditionalKey(2f));
    }
    public IEnumerator HideUnlockAdditionalKey(float timeToHide) // Faire apparaitre un morceau de clé puis faire disparaitre l'UI
    {
        yield return new WaitForSeconds(timeToHide+1.5f);
        addShard.DOAnchorPos(showBonusPosition, 0.5f);
        yield return new WaitForSeconds(1.5f);
        lockImage.DOFade(0,0.5f);
        yield return new WaitForSeconds(1f);
        lockedFilter.DOFade(0,0.5f);

        yield return new WaitForSeconds(timeToHide+1.5f);
        addShard.DOAnchorPos(hideBonusPosition, 0.5f).OnComplete((() =>   MapManager.instance.IconMapUpdate(3)));
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
        compteur.text = currentShard + " / 6"; 
        myRect.DOAnchorPos(showPosition, 0.5f);
    }
    
    public IEnumerator ShowMapKeyWithDelay(float delay) // Faire apparaite et disparaitre l'UI dans le menu de la carte
    {
        yield return new WaitForSeconds(delay-0.5f);
        compteur.text = currentShard + " / 6"; 
        myRect.DOAnchorPos(showPosition, 0.5f);
        yield return new WaitForSeconds(0.3f);
        CarnetManager.instance.canOpen = true;
    }
    
    public void HideMapKey()
    {
        myRect.DOAnchorPos(hidePosition, 0.5f);
    }
    
    public IEnumerator ShowMapBonusKeyWithDelay(float delay) // Faire apparaite et disparaitre l'UI dans le menu de la carte
    {
        yield return new WaitForSeconds(delay-0.5f);
            compteurBonus.text = currentBonusShard + " / 4"; 
            addShard.DOAnchorPos(showBonusPosition, 0.5f);
            yield return new WaitForSeconds(0.3f);
            CarnetManager.instance.canOpen = true;
    }
    
    public void HideMapBonusKey()
    {
        addShard.DOAnchorPos(hideBonusPosition, 0.5f);
    }
}
