using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Playables;

public class CinématiqueManager : MonoBehaviour
{
    public bool enableCinematics;
    public PlayableDirector cinematiqueManager;
    public List<PlayableAsset> cinématiqueList;
    public float cinématiqueTime;
    public List<GameObject> objetsCinématqiue;
    public static CinématiqueManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (enableCinematics)
        {
            StartCoroutine(CinématiqueIntro());
        }
    }

    public IEnumerator CinématiqueIntro()
    {
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
     
        foreach (GameObject oui in objetsCinématqiue)
        {
            Destroy(oui);
        }
        objetsCinématqiue.Clear();
       
    }
    
    public IEnumerator CinématiqueBOTW()
    {
        cinematiqueManager.playableAsset = cinématiqueList[1];
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        cinematiqueManager.Play();
        yield return new WaitForSeconds((float)cinematiqueManager.duration);
        cinematiqueManager.Stop();
        CameraController.instance.canMove = true;
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
    }
    
}
