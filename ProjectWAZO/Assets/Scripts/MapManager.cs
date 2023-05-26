using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public bool MapGot;
    public bool isRotated;
    public bool canRotate;
    public TextMeshProUGUI textIndication;
    public CanvasGroup MapMenu;
    public RectTransform iconMapUpdate;
    public RectTransform iconMapFound;
    public Image Map;
    public Sprite mapPleine;
    public Sprite mapRetournee;
    public GameObject mapElements;
    public GameObject loreElements;
    public List<Image> pontIntero;
    public List<Image> listCroix;
    public List<Image> doneFilterList;
    public List<Image> fresquesList;
    public List<Image> lockList;

    [Header("PlayerIcon")]
    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private float worldHeight;
    [SerializeField] private float worldWidth;
    [SerializeField] private float heightOffset;
    [SerializeField] private float widthOffset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (MapGot)
        {
            Map.sprite = null;
        }
        else
        {
            Map.sprite = mapPleine;
        }
    }
    
    public IEnumerator RotateMap()
    {
        if (CarnetManager.instance.isOpened && canRotate)
        {
            if (!isRotated)
            {
                textIndication.text = "Map";
                isRotated = true;
                canRotate = false;
                CarnetManager.instance.canOpen = false;
                Map.transform.DORotate(new Vector3(0, 90, 0),0.3f);
                yield return new WaitForSeconds(0.3f);
                playerIcon.gameObject.SetActive(false);
                mapElements.SetActive(false);
                Map.sprite = mapRetournee;
                loreElements.SetActive(true);
                Map.transform.DORotate(new Vector3(0, 0, 0), 0.3f).OnComplete((() =>    CarnetManager.instance.canOpen = false));
                yield return new WaitForSeconds(0.3f);
                canRotate = true;
            }
            else
            {
                textIndication.text = "Forgoten Frescos";
                isRotated = false;
                canRotate = false;
                Map.transform.DORotate(new Vector3(0, 90, 0),0.3f);
                yield return new WaitForSeconds(0.3f);
                playerIcon.gameObject.SetActive(true);
                mapElements.SetActive(true);
                Map.sprite = mapPleine;
                loreElements.SetActive(false);
                Map.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
                yield return new WaitForSeconds(0.3f);
                canRotate = true;
            }
        }
    }

    #region Update et Found Map

    public void IconMapFound(float duration2)
    {
        StartCoroutine(ShowAndHide(duration2));
    }

    IEnumerator ShowAndHideFound(float duration2)
    {
        iconMapFound.gameObject.SetActive(true);
        iconMapFound.DOAnchorPosX(-15, 0.5f);
        yield return new WaitForSeconds(duration2);
        iconMapFound.DOAnchorPosX(200, 0.5f).OnComplete((() =>   iconMapFound.gameObject.SetActive(false)));
      
    }
    
    public void IconMapUpdate(float duration)
    {
        StartCoroutine(ShowAndHide(duration));
    }

    IEnumerator ShowAndHide(float duration)
    {
        iconMapUpdate.gameObject.SetActive(true);
        iconMapUpdate.DOAnchorPosX(-15, 0.5f);
        yield return new WaitForSeconds(duration);
        iconMapUpdate.DOAnchorPosX(200, 0.5f).OnComplete((() =>   iconMapUpdate.gameObject.SetActive(false)));
      
    }

    #endregion
  

    public void UnlockFresque(int ID)
    {
        fresquesList[ID].DOColor(Color.white, 0.5f);
        if (lockList[ID] is not null)
        {
            Destroy(lockList[ID].gameObject);
        }
    }
    
    public void MovePlayerIcon()
    {
        if (!isRotated)
        {
            var worldPos = Controller.instance.transform.position;
            var mapPos = new Vector2(worldPos.x + widthOffset, worldPos.z + heightOffset);
            playerIcon.anchoredPosition = Vector2.Scale(mapPos, Vector2.Scale(Map.rectTransform.rect.size, new Vector2(1/worldWidth, 1/worldHeight)));
        }
    }
}
