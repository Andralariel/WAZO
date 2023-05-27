using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using EventSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CinématiqueManager : MonoBehaviour
{
    public bool enableStartCinematic;
    public bool isCinématique;
    public PlayableDirector cinematiqueManager;
    public List<PlayableAsset> cinématiqueList;
    private List<IEnumerator> coroutineList = new List<IEnumerator>();
    public static CinématiqueManager instance;
    public BoxCollider box;
    public Volume globalVolume;
    private DepthOfField dof = null;

    [Header("CineCarte")] 
    public float playerSpeed;
    public List<GameObject> spiritsList;
    public List<ParticleSystem> spawnVfxList;
    public ParticleSystem mapVfx;
    public GameObject modeMap;
    public GameObject playerCenterPoint;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        coroutineList.Add(CinématiqueIntro());
        coroutineList.Add(CinématiqueBOTW());
        coroutineList.Add(CinématiqueCarte());
    }

    void Start()
    {
        //globalVolume.profile.TryGetSubclassOf(VolumeComponent,out dof);
        if (enableStartCinematic)
        {
            StartCoroutine(CinématiqueIntro());
        }
    }

    public void StartCinématique(int cinématqueIndex)
    {
        StartCoroutine(coroutineList[cinématqueIndex]);
        Debug.Log("tentative de cinématqiue");
    }
    
    public IEnumerator CinématiqueIntro() // index 0
    {
        isCinématique = true;
        cinematiqueManager.playableAsset = cinématiqueList[0];
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        cinematiqueManager.Play();
        yield return new WaitForSeconds((float)cinematiqueManager.duration);
        cinematiqueManager.Stop();
        KeyUI.instance.FadeInBlackScreen(0.5f);
        yield return new WaitForSeconds(0.5f);
        CameraController.instance.canMove = true;
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
        yield return new WaitForSeconds(2);
        KeyUI.instance.FadeOutBlackScreen(1);
        isCinématique = false;
    }

    public IEnumerator CinématiqueBOTW() // index 1
    {
        Controller.instance.isGoing = true;
        Controller.instance.thingToLook = modeMap;
        Controller.instance.pointToGo = playerCenterPoint;
        Controller.instance.cineSpeed = 1f;
        PauseMenu.instance.canPause = false;
        yield return new WaitForSeconds(1.5f);
        CameraController.instance.isFlou = false;
        isCinématique = false;
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        PauseMenu.instance.canPause = false;
        CarnetManager.instance.canOpen = false;
     
        /*Controller.instance.moveInput = Vector3.zero;
        Controller.instance.anim.SetBool("isWalking",false);
        Controller.instance.anim.SetBool("isFlying",false);
        Controller.instance.anim.SetBool("isIdle",true);*/
        if (!Controller.instance.isGrounded)
        {
            Controller.instance.ultraBlock = true;
        }
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        
        
        cinematiqueManager.playableAsset = cinématiqueList[1];
        Controller.instance.anim.SetBool("isWalking",false);
        Controller.instance.anim.SetBool("isFlying",false);
        Controller.instance.anim.SetBool("isIdle",true);
        cinematiqueManager.Play();
        yield return new WaitForSeconds((float)cinematiqueManager.duration - 1);
        CameraController.instance.isFlou = true;
        yield return new WaitForSeconds(1);
        cinematiqueManager.Stop();
        yield return new WaitForSeconds(0.5f);
        CameraController.instance.SmoothMoveFactor = 0.5f;
        CameraController.instance.canMove = true;
        isCinématique = false;
        globalVolume.weight = 1;
        StartCoroutine(CinématiqueCarte());
    }

    public IEnumerator CinématiqueCarte()
    {
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        Controller.instance.ultraBlock = true;
        yield return new WaitForSeconds(1.5f);
        CameraController.instance.SmoothMoveFactor = 1f;
        CameraController.instance.offset = new Vector3(2, 10, -3.5f);
        yield return new WaitForSeconds(0.8f);
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*5, 3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*3, 19f);
        for (int i = 0; i < spiritsList.Count; i++) // fait spawn les esprits
        {
            spawnVfxList[i].Play();
            yield return new WaitForSeconds(0.1f);
            spiritsList[i].SetActive(true);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(3f);  // fait spawn la carte
        mapVfx.Play();
        modeMap.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        modeMap.transform.DOMove(Controller.instance.transform.position + new Vector3(0,0.5f,0), 2f);
        modeMap.transform.DOScale(new Vector3(0,0,0), 2.5f);
        yield return new WaitForSeconds(6f);
        for (int i = 0; i < spiritsList.Count; i++) // fait despawn les esprits
        {
            spawnVfxList[i].Play();
            yield return new WaitForSeconds(0.1f);
            spiritsList[i].SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(3f);
        Controller.instance.isGoing = false;
        CameraController.instance.offset = new Vector3(4, 15, -8.5f);
        box.enabled = true;
        MapManager.instance.Map.sprite = MapManager.instance.mapPleine;
        MapManager.instance.MapGot = true;
        CarnetManager.instance.canOpen = true;
        yield return new WaitForSeconds(1f);
        CameraController.instance.SmoothMoveFactor = 0.2f;
    }
    
    
    //-------------------------------Cinématique carte sans le système de déplacement d'esprits------------------------------------------
    /*for (int i = 0; i < 14; i++)
       {
           spiritList[i].SetActive(true);
           spiritList[i].transform.DOMove(wayPoints[i].transform.position, 4f);
       }
       yield return new WaitForSeconds(5f);
       vfxSpawn.Play();
       modeMap.SetActive(true);
       yield return new WaitForSeconds(3f);
       modeMap.transform.DOMove(Controller.instance.transform.position + new Vector3(0,0.5f,0), 3f);
       modeMap.transform.DOScale(new Vector3(0,0,0), 4f);
       for (int i = 0; i < 7; i++)
       {
           spiritList[i].transform.DOMove(spawnPointG.transform.position, 5f);
       }
       for (int i = 7; i < 14; i++)
       {
           spiritList[i].transform.DOMove(spawnPointD.transform.position, 5f);
       }
       yield return new WaitForSeconds(4.5f);
       for (int i = 0; i < 14; i++)
       {
           spiritList[i].SetActive(false);
       }*/
    
    
    
    //-------------------------------Cinématique carte avec le système de déplacement d'esprits------------------------------------------
    /*yield return new WaitForSeconds(2f);
      gaucheManager.OnEventActivate();
      droiteManager.OnEventActivate();
      yield return new WaitForSeconds(2f);
      vfxSpawn.Play();
      modeMap.SetActive(true);
      yield return new WaitForSeconds(1.5f);
      modeMap.transform.DOMove(Controller.instance.transform.position + new Vector3(0,0.5f,0), 3f);
      modeMap.transform.DOScale(new Vector3(0,0,0), 4f);*/
}


