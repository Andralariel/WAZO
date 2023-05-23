using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    
    public static MapManager instance;
    public bool MapGot;
    public bool isMenuOpened;
    public CanvasGroup MapMenu;
    public RectTransform iconMapUpdate;
    public Image Map;
    public Sprite mapPleine;
    public List<Image> pontIntero;
    public List<Image> listCroix;
    public List<Image> doneFilterList;
    public List<Image> fresquesList;

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

    public void MovePlayerIcon()
    {
        var worldPos = Controller.instance.transform.position;
        var mapPos = new Vector2(worldPos.x + widthOffset, worldPos.z + heightOffset);
        playerIcon.anchoredPosition = Vector2.Scale(mapPos, Vector2.Scale(Map.rectTransform.rect.size, new Vector2(1/worldWidth, 1/worldHeight)));
    }
}
