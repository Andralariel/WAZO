
using System.Collections;
using System.Collections.Generic;
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
    public Volume globalVolume;
    private DepthOfField dof = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        coroutineList.Add(CinématiqueIntro());
        coroutineList.Add(CinématiqueBOTW());
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
        globalVolume.weight = 0;
        //dof.enabled.value = false;
            isCinématique = false;
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        Controller.instance.moveInput = Vector3.zero;
        Controller.instance.anim.SetBool("isWalking",false);
        Controller.instance.anim.SetBool("isFlying",false);
        Controller.instance.anim.SetBool("isIdle",true);
        if (!Controller.instance.isGrounded)
        {
            Controller.instance.ultraBlock = true;
        }
        cinematiqueManager.playableAsset = cinématiqueList[1];
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        Controller.instance.anim.SetBool("isWalking",false);
        Controller.instance.anim.SetBool("isFlying",false);
        Controller.instance.anim.SetBool("isIdle",true);
        cinematiqueManager.Play();
        yield return new WaitForSeconds((float)cinematiqueManager.duration);
        cinematiqueManager.Stop();
        MapManager.instance.Map.sprite = MapManager.instance.mapPleine;
        MapManager.instance.MapGot = true;
        Controller.instance.ultraBlock = false;
        CameraController.instance.canMove = true;
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
        globalVolume.weight = 1;
        isCinématique = false;
    }
}
