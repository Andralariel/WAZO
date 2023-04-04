using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Playables;

public class CinématiqueManager : MonoBehaviour
{
    public bool enableCinematics;
    public PlayableDirector cinematique1;
    public float cinématiqueTime;
    public List<GameObject> objetsCinématqiue;

    void Start()
    {
        if (enableCinematics)
        {
            StartCoroutine(CinématiqueIntro());
        }
    }

    IEnumerator CinématiqueIntro()
    {
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        cinematique1.Play();
        yield return new WaitForSeconds(cinématiqueTime);
        cinematique1.Stop();
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
    
}
