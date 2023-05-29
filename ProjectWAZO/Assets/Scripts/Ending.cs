using System;
using System.Collections;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
    public Transform PointToGo2;
    public Transform CameraPoint;
    public GameObject crédits;
    public Image BlackScreen;
    public float creditSpeed;
    public float timeToEnd;
    public float timeToGo;
    private bool rollCredits;
    public float cameraSpeed1;
    public float cameraSpeed2;
    public float playerSpeed;
    public AudioSource earthquakeSound;
    [SerializeField] private float fadeDuration;

    private void Update()
    {
        if (rollCredits)
        {
            crédits.transform.position += new Vector3(0, creditSpeed, 0)*Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            player.isGoing = true;
            player.pointToGo = PointToGo.gameObject;
            player.thingToLook = thingToLook.gameObject;
            player.cineSpeed = 0.8f;
            CameraController.instance.transform.DOMoveX(CameraPoint.position.x,0.2f).SetEase(Ease.Linear)
                .OnComplete( (() => CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward* 11.1f + 
                                                                               CameraController.instance.transform.forward*3, 2.5f)));
            CameraController.instance.SmoothMoveFactor = 0.8f;
            StartCoroutine(EndCinématic());
        }
    }

    IEnumerator EndCinématic()
    {
        yield return new WaitForSeconds(timeToGo);
        CameraController.instance.camShake = false;
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward * cameraSpeed1, 4f);
        player.canPlaner = true;
        player.isPressing = true;
        player.planingGravity = 6;
        player.pointToGo = PointToGo2.gameObject;
        player.rb.AddForce(new Vector3(0,20,15),ForceMode.Impulse);
        yield return new WaitForSeconds(4f);
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward * cameraSpeed2, 4f);
        player.planingGravity = -2;
        earthquakeSound.DOFade(0f, fadeDuration).OnComplete(() => earthquakeSound.Stop());
        yield return new WaitForSeconds(4f);
        CameraController.instance.canMove = true;
        CameraController.instance.SmoothMoveFactor = 1.5f;
        CameraController.instance.offset = new Vector3(8.5f,10f,1.5f);
        player.MoveEnding = true;
        player.StopGravity = true;
        player.rb.useGravity = false;
        player.planingGravity = 0;
        yield return new WaitForSeconds(5f);
        crédits.gameObject.SetActive(true);
        rollCredits = true;
        yield return new WaitForSeconds(timeToEnd);
        CameraController.instance.SmoothMoveFactor = 10;
        CameraController.instance.offset = new Vector3(8.5f,10f,-100.5f);
        BlackScreen.DOFade(1, 3f);
        yield return new WaitForSeconds(3.2f);
        SceneManager.LoadScene("Dev_CinematicIntro");
    }
}
