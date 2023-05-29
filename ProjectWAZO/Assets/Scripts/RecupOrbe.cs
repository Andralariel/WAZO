using System;
using System.Collections;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RecupOrbe : MonoBehaviour
{
    public GameObject orbe;
    [SerializeField] private ParticleSystem vfxreplacing;
    [SerializeField] private ParticleSystem basic;
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
    public GameObject Eboulement;
    public GameObject Eboulement2;
    public GameObject Eboulement3;
    public float timeToGo;
    public AudioSource earthquakeSound;
    private float graphValue;
    public AnimationCurve curveIntensity;
    private bool orbed;
    [SerializeField] private VolumeProfile v;
    private ChromaticAberration c;
    private float time;
    private void Start()
    {
        time = 0;
        orbed = false;
        v.TryGet(out c);
    }
    void Update()
    {
        if (orbed)
        {
            time ++;
            graphValue = curveIntensity.Evaluate(time/100);
            c.intensity.value = graphValue;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Controller.instance.isGoing = true;
            Controller.instance.pointToGo = PointToGo.gameObject;
            Controller.instance.cineSpeed = 0.8f;
            Controller.instance.thingToLook = thingToLook;
            CinématiqueManager.instance.isCinématique = true;
            AudioList.Instance.PlayOneShot(AudioList.Instance.cinematiqueOrbe, AudioList.Instance.cinematiqueOrbeVolume);
            StartCoroutine(EndCinématic());
        }
    }

    IEnumerator EndCinématic()
    {
        yield return new WaitForSeconds(timeToGo);
        player.isGoing = false;
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*5, 8f);
        yield return new WaitForSeconds(2);
        orbe.transform.DOMove(Controller.instance.transform.position, 3f);
        yield return new WaitForSeconds(1.5f);
        vfxreplacing.Play();
        orbed = true;
        basic.Stop();
        yield return new WaitForSeconds(4.5f);
        orbed = true;
        CameraController.instance.camShake = true;
        earthquakeSound.Play();
        yield return new WaitForSeconds(2f);
        Eboulement.SetActive(true);
        Eboulement2.SetActive(true);
        Eboulement3.SetActive(true);
        Eboulement.transform.DOMove(new Vector3(Eboulement.transform.position.x, Eboulement.transform.position.y - 20, 
            Eboulement.transform.position.z), 0.5f);
        Eboulement2.transform.DOMove(new Vector3(Eboulement2.transform.position.x, Eboulement2.transform.position.y - 20, 
            Eboulement2.transform.position.z), 0.2f);
        Eboulement3.transform.DOMove(new Vector3(Eboulement3.transform.position.x, Eboulement3.transform.position.y - 20, 
            Eboulement.transform.position.z), 0.4f);
        yield return new WaitForSeconds(0.3f);
        AudioList.Instance.PlayOneShot(AudioList.Instance.fallingRock, AudioList.Instance.fallingRockVolume);
        yield return new WaitForSeconds(2f);
        CinématiqueManager.instance.isCinématique = false;
        player.canMove = true;
        player.canJump = true;
        player.enabled = true;
        Destroy(gameObject);
    }
    
    
}
