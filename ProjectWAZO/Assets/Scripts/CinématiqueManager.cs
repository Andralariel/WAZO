using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Playables;

public class CinématiqueManager : MonoBehaviour
{
    public bool enableStartCinematic;
    public bool isCinématique;
    public PlayableDirector cinematiqueManager;
    public List<PlayableAsset> cinématiqueList;
    public List<GameObject> objetsCinématique;
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
        if (enableStartCinematic)
        {
            StartCoroutine(CinématiqueIntro());
        }
    }

    public IEnumerator CinématiqueIntro()
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
     
        foreach (GameObject oui in objetsCinématique)
        {
            Destroy(oui);
        }
        objetsCinématique.Clear();
        isCinématique = false;
    }
    
    public IEnumerator CinématiqueBOTW()
    {
        isCinématique = true;
        cinematiqueManager.playableAsset = cinématiqueList[1];
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = false;
        cinematiqueManager.Play();
        Debug.Log((float)cinematiqueManager.duration);
        yield return new WaitForSeconds(25);
        MapManager.instance.Map.sprite = MapManager.instance.mapPleine;
        isCinématique = false;
        CameraController.instance.canMove = true;
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
    }
    
}
