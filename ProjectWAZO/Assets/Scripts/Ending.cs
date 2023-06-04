using System;
using System.Collections;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Ending : MonoBehaviour
{
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
    public GameObject crédits;
    public float creditSpeed;
    public float timeToGo;
    public bool rollCredits;
    public bool isCiné;
    public Image BlackScreen;
    public PlayableDirector timeLine;
    public GameObject CameraJeu;
    public GameObject CameraCinématque;
    public AudioSource earthquakeSound;
    [SerializeField] private float fadeDuration;
    private float graphValue;
    public AnimationCurve curveChromatic;
    public AnimationCurve curveSaturation;
    public AnimationCurve curveBloom;
    public AnimationCurve curveBloomT;
    [SerializeField] private VolumeProfile v;
    private ChromaticAberration c;
    private ColorAdjustments ca;
    private Bloom b;
    private float time;
    private bool jumped;

    [Header("Timers")] 
    public float timeToCrédits;
    public float timeToEnd;
    private void Start()
    {
        time = 0;
        jumped = false;
        v.TryGet(out c);
        v.TryGet(out ca);
        v.TryGet(out b);
    }

    private void Update()
    {
        if (jumped)
        {
            time += Time.deltaTime;
            graphValue = curveChromatic.Evaluate(time/3);
            c.intensity.value = graphValue;
            graphValue = curveSaturation.Evaluate(time/3);
            ca.saturation.value = graphValue;
            graphValue = curveBloom.Evaluate(time/3);
            b.intensity.value = graphValue;
            graphValue = curveBloomT.Evaluate(time/3);
            b.threshold.value = graphValue;
        }
        
        if (rollCredits)
        {
            crédits.transform.position += new Vector3(0, creditSpeed, 0)*Time.deltaTime;
        }

        if (isCiné)
        {
            Debug.Log(Vector3.Distance( PointToGo.transform.position,Controller.instance.transform.position));
            if (Controller.instance.isGoing == false)
            {
                StartCoroutine(EndCinématic()); 
            }
        }
    }

    //Vector3.Distance( PointToGo.transform.position,Controller.instance.transform.position) <= 0.5f
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            player.isGoing = true;
            player.pointToGo = PointToGo.gameObject;
            player.thingToLook = thingToLook.gameObject;
            Vector2 dist = PointToGo.transform.position - player.transform.position;
            player.cineSpeed = 1;
            isCiné = true;
            CameraController.instance.canMove = false;
            //CameraController.instance.camShake = false;
            CameraController.instance.SmoothMoveFactor = 0.2f;
            CameraController.instance.transform.DOMove(new Vector3(46.52f,38.95f,103.79f),timeToGo);
            CameraController.instance.transform.DORotate(new Vector3(20,0,0),timeToGo);
        }
    }
    
    
    IEnumerator EndCinématic()
    {
        CameraController.instance.camShake = false;
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        Controller.instance.ultraBlock = true;
        CameraJeu.SetActive(false);
        earthquakeSound.DOFade(0f, fadeDuration).OnComplete(() => earthquakeSound.Stop());
        CameraCinématque.SetActive(true);
        timeLine.Play();
        jumped = true;
        yield return new WaitForSeconds(timeToCrédits);
        rollCredits = true;
        crédits.SetActive(true);
        yield return new WaitForSeconds(timeToEnd);
        BlackScreen.DOFade(1, fadeDuration);
        yield return new WaitForSeconds(fadeDuration+1f);
        SceneManager.LoadScene("Dev_CinematicIntro");

    }
}


/*player.rb.AddForce(new Vector3(0,20,15),ForceMode.Impulse);
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
      SceneManager.LoadScene("Dev_CinematicIntro");*/
